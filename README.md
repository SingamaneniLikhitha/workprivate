public static List<FileEntity> getUploadReportForZipFile(File zipFile, PLPSessionContext plp_context, String clientId) throws Exception {

    List<FileEntity> entityList = new ArrayList<>();
    String dirPath = getUploadFilePath(clientId);
    ZipFile zf = null;

    try {
        zf = new ZipFile(zipFile);

        for (Enumeration entries = zf.entries(); entries.hasMoreElements(); ) {
            ZipEntry entry = (ZipEntry) entries.nextElement();
            String entryName = entry.getName();

            // NEW CHANGE: Zip Slip protection
            Path baseDirPath = Paths.get(dirPath).toAbsolutePath().normalize(); // NEW CHANGE
            Path resolvedPath = baseDirPath.resolve(entryName).normalize();     // NEW CHANGE
            if (!resolvedPath.startsWith(baseDirPath)) {                        // NEW CHANGE
                throw new SecurityException("Invalid zip entry: " + entryName); // NEW CHANGE
            }

            // NEW CHANGE: Extract safe file name only
            String fileName = Paths.get(entryName).getFileName().toString();    // NEW CHANGE

            Long fileSize = entry.getSize();
            Date fileDate = new Date(entry.getTime());
            InputStream is = zf.getInputStream(entry);
            String batchId = getBatchId();
            String internalFileName = generateUniqueFileName(fileName, plp_context.clientID, plp_context.webProfileID, batchId);
            String finalFilePath = finalCSVFilePathWithFileName(dirPath, internalFileName);

            if (!isZipFile(getFileType(fileName))) {
                createFile(is, finalFilePath);
                FileEntity fileEntity = getUploadReportForSimpleFile(fileName, internalFileName, fileSize, fileDate, plp_context.userID, clientId, plp_context.webProfileID, batchId);
                entityList.add(fileEntity);
            } else {
                String dirPath_temp = getTempDirPath(clientId);
                String tempFileName = generateUniqueTempFileName(fileName);
                File tempFile = new File(dirPath_temp, tempFileName);

                // NEW CHANGE: Validate temp file path
                Path tempFilePath = tempFile.toPath().normalize();              // NEW CHANGE
                Path tempBasePath = new File(dirPath_temp).toPath().normalize(); // NEW CHANGE
                if (!tempFilePath.startsWith(tempBasePath)) {                  // NEW CHANGE
                    throw new SecurityException("Invalid temp file path: " + tempFileName); // NEW CHANGE
                }

                File createdTempFile = createFile(is, tempFile.getAbsolutePath());
                entityList.addAll(getUploadReportForZipFile(createdTempFile, plp_context, clientId));
                createdTempFile.delete();
            }
        }

        zf.close();
    } catch (Exception e) {
        throw new Exception("PatientList,getUploadReport \nZip file: " + zipFile + "\nClient. id: " + plp_context.clientID, e);
    } finally {
        if (zf != null) {
            try {
                zf.close();
            } catch (IOException e) {
                throw e;
            }
        }
    }

    return entityList;
}
