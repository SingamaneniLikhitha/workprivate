private List<FileEntity> getUploadReportForZipFile(File zipFile, PLPSessionContext plp_context, String clientId) throws Exception {
    String txType = CommonUtil.isBlankString(CoreUtil.getRealTimeTXIdByAppID(plp_context.serviceID))
            ? "ELG"
            : CoreUtil.getRealTimeTXIdByAppID(plp_context.serviceID);

    List<FileEntity> entityList = new ArrayList<>();
    String dirPath = RTUtil.getUploadFilePath(clientId);
    BaseConfig rtConfig = plp_context.config;

    try (ZipFile zf = new ZipFile(zipFile)) {
        for (Enumeration entries = zf.entries(); entries.hasMoreElements(); ) {
            ZipEntry entry = (ZipEntry) entries.nextElement();
            String entryName = entry.getName(); // ✅ NEW CHANGE

            // ✅ NEW CHANGE: Zip Slip protection
            Path baseDirPath = Paths.get(dirPath).toAbsolutePath().normalize();
            Path resolvedPath = baseDirPath.resolve(entryName).normalize();
            if (!resolvedPath.startsWith(baseDirPath)) {
                throw new SecurityException("Bad zip entry: " + entryName);
            }

            // ✅ NEW CHANGE: Extract safe file name only
            String fileName = Paths.get(entryName).getFileName().toString();

            Long fileSize = entry.getSize();
            Date fileDate = new Date(entry.getTime());
            InputStream is = zf.getInputStream(entry);

            String internalFileName = RTUtil.generateUniqueFileName(fileName, txType, plp_context.userID, rtConfig.userType);
            String finalFilePath = RTUtil.finalRTCSVFilePathWithFileName(dirPath, internalFileName);

            if (!RTUtil.isZipFile(RTUtil.getFileType(fileName))) {
                RTUtil.createFile(is, finalFilePath);
                FileEntity fileEntity = getUploadReportForSimpleFile(
                        fileName, internalFileName, fileSize, fileDate,
                        plp_context.userID, clientId, plp_context.webProfileID
                );
                entityList.add(fileEntity);
            } else {
                String dirPath_temp = RTUtil.getTempDirPath(clientId);
                String tempFileName = RTUtil.generateUniqueTempFileName(fileName);
                File tempFile = new File(dirPath_temp, tempFileName);

                // ✅ NEW CHANGE: Validate temp file path
                Path tempFilePath = tempFile.toPath().normalize();
                Path tempBasePath = new File(dirPath_temp).toPath().normalize();
                if (!tempFilePath.startsWith(tempBasePath)) {
                    throw new SecurityException("Invalid temp file path: " + tempFileName);
                }

                File createdTempFile = RTUtil.createFile(is, tempFile.getAbsolutePath());
                entityList.addAll(getUploadReportForZipFile(createdTempFile, plp_context, clientId));
                createdTempFile.delete();
            }
        }
    } catch (Exception e) {
        throw new Exception(
                new StringBuilder(this.getClass().getSimpleName())
                        .append(",getUploadReport\nZip file: ").append(zipFile)
                        .append("\nClient. id: ").append(plp_context.clientID)
                        .toString(), e
        );
    }

    return entityList;
}
