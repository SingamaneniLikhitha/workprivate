private static String getInClausePlaceholders(String[] values, List<Object[]> params, int sqlType) {
 for (String val : values) {
 params.add(new Object[]{sqlType, val});
 }
 return String.join(",", Collections.nCopies(values.length, "?")); // ?,?,?
}


switch (eligStatus) {

case "Active": {

String placeholders = getInClausePlaceholders(CoreConstants.ELG_STATUS_ACTIVE, setParameters, Types.VARCHAR);

String condition = "c1.last_elig_status IN (" + placeholders + ")";

RevenueCycleMgmtUtil.create_sql_fragment_no_params(condition, whereClause, and_operator);

break;

}



case "InActive": {

String placeholders = getInClausePlaceholders(CoreConstants.ELG_STATUS_INACTIVE, setParameters, Types.VARCHAR);

String condition = "c1.last_elig_status IN (" + placeholders + ")";

RevenueCycleMgmtUtil.create_sql_fragment_no_params(condition, whereClause, and_operator);

break;

}



case "Validation": {

String placeholders = getInClausePlaceholders(CoreConstants.ELG_STATUS_ERROR, setParameters, Types.VARCHAR);

String condition = "(c1.last_elig_status LIKE '%_AAA_%' AND c1.last_elig_status NOT IN (" + placeholders + "))";

RevenueCycleMgmtUtil.create_sql_fragment_no_params(condition, whereClause, and_operator);

break;

}



case "InactiveAndValidation": {

String inactive = getInClausePlaceholders(CoreConstants.ELG_STATUS_INACTIVE, setParameters, Types.VARCHAR);

String error = getInClausePlaceholders(CoreConstants.ELG_STATUS_ERROR, setParameters, Types.VARCHAR);

String condition = "(c1.last_elig_status IN (" + inactive + ") OR (c1.last_elig_status LIKE '%_AAA_%' AND c1.last_elig_status NOT IN (" + error + ")))";

RevenueCycleMgmtUtil.create_sql_fragment_no_params(condition, whereClause, and_operator);

break;

}



default: {

String placeholders = getInClausePlaceholders(CoreConstants.ELG_STATUS_ERROR, setParameters, Types.VARCHAR);

String condition = "(c1.last_elig_status IN (" + placeholders + "))";

RevenueCycleMgmtUtil.create_sql_fragment_no_params(condition, whereClause, and_operator);

break;

}

}

