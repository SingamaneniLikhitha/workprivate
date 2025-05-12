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
--------------------NEW----------------
if (remitStatus != null)
				{
					and_operator_temp = true;
                    switch (remitStatus) {
                        case "Remittance":
							RevenueCycleMgmtUtil.createnewSQLFragment("c1.last_era_status IN (" + String.join(",",Collections.nCopies(Constants.ERA_STATUS_PAID.length,"?")) + ")",Types.ARRAY,Constants.ERA_STATUS_PAID,whereClause,setParameters,!and_operator);
//							RevenueCycleMgmtUtil.create_sql_fragment_no_params(" c1.last_era_status IN (" + getCommaSeparatedValues(Constants.ERA_STATUS_PAID) + ")", whereClause, and_operator);
                            break;
                        case "NoRemittance":
							RevenueCycleMgmtUtil.createnewSQLFragment("c1.last_era_status not IN (" + String.join(",",Collections.nCopies(Constants.ERA_STATUS_PAID.length,"?")) + ")",Types.ARRAY,Constants.ERA_STATUS_PAID,whereClause,setParameters,!and_operator);
//							RevenueCycleMgmtUtil.create_sql_fragment_no_params(" (c1.last_era_status not IN (" + getCommaSeparatedValues(Constants.ERA_STATUS_PAID) + ") OR c1.last_era_status IS NULL)", whereClause, and_operator);
                            break;
                        case "ZeroPaid":
                            RevenueCycleMgmtUtil.getWhereClauseFragment(whereClause, setParameters, !and_operator, Types.VARCHAR, "c1.last_era_status = ?", Constants.ERA_STATUS_ZERO_PAID);
                            break;
                        case "NonZeroPaid":
                            RevenueCycleMgmtUtil.getWhereClauseFragment(whereClause, setParameters, !and_operator, Types.VARCHAR, "c1.last_era_status = ?", Constants.ERA_STATUS_NON_ZERO_PAID);
                            break;
                        default:
                            and_operator_temp = and_operator;
                            break;
                    }
					and_operator = and_operator_temp;
				}



=== FRUIT 🍉🍎🍊🍐🍋‍🟩 ===

if (remitStatus != null) {
    and_operator_temp = true;
    switch (remitStatus) {

        case "Remittance": {
            String placeholders = getInClausePlaceholders(Constants.ERA_STATUS_PAID, setParameters, Types.VARCHAR);
            String condition = "c1.last_era_status IN (" + placeholders + ")";
            RevenueCycleMgmtUtil.create_sql_fragment_no_params(condition, whereClause, and_operator);
            break;
        }

        case "NoRemittance": {
            String placeholders = getInClausePlaceholders(Constants.ERA_STATUS_PAID, setParameters, Types.VARCHAR);
            String condition = "(c1.last_era_status NOT IN (" + placeholders + ") OR c1.last_era_status IS NULL)";
            RevenueCycleMgmtUtil.create_sql_fragment_no_params(condition, whereClause, and_operator);
            break;
        }

        case "ZeroPaid": {
            RevenueCycleMgmtUtil.getWhereClauseFragment(
                whereClause, setParameters, !and_operator,
                Types.VARCHAR, "c1.last_era_status = ?", Constants.ERA_STATUS_ZERO_PAID);
            break;
        }

        case "NonZeroPaid": {
            RevenueCycleMgmtUtil.getWhereClauseFragment(
                whereClause, setParameters, !and_operator,
                Types.VARCHAR, "c1.last_era_status = ?", Constants.ERA_STATUS_NON_ZERO_PAID);
            break;
        }

        default: {
            and_operator_temp = and_operator;
            break;
        }
    }
    and_operator = and_operator_temp;
}
	

	
