if (remitStatus != null) {
    and_operator_temp = true;

    switch (remitStatus) {
        case "Remittance": {
            String placeholders = String.join(",", Collections.nCopies(Constants.ERA_STATUS_PAID.length, "?"));
            String condition = "c1.last_era_status IN (" + placeholders + ")";
            RevenueCycleMgmtUtil.createnewSQLFragment(
                condition,
                Types.VARCHAR,
                Constants.ERA_STATUS_PAID,
                whereClause,
                setParameters,
                !and_operator
            );
            break;
        }

        case "NoRemittance": {
            String placeholders = String.join(",", Collections.nCopies(Constants.ERA_STATUS_PAID.length, "?"));
            String condition = "(c1.last_era_status NOT IN (" + placeholders + ") OR c1.last_era_status IS NULL)";
            RevenueCycleMgmtUtil.createnewSQLFragment(
                condition,
                Types.VARCHAR,
                Constants.ERA_STATUS_PAID,
                whereClause,
                setParameters,
                !and_operator
            );
            break;
        }

        case "ZeroPaid": {
            RevenueCycleMgmtUtil.getWhereClauseFragment(
                whereClause,
                setParameters,
                !and_operator,
                Types.VARCHAR,
                "c1.last_era_status = ?",
                Constants.ERA_STATUS_ZERO_PAID
            );
            break;
        }

        case "NonZeroPaid": {
            RevenueCycleMgmtUtil.getWhereClauseFragment(
                whereClause,
                setParameters,
                !and_operator,
                Types.VARCHAR,
                "c1.last_era_status = ?",
                Constants.ERA_STATUS_NON_ZERO_PAID
            );
            break;
        }

        default: {
            and_operator_temp = and_operator;
            break;
        }
    }

    and_operator = and_operator_temp;
}
