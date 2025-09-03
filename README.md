1. Assurance ERA–Claim Linking
How It Works

Matching Criteria:

Assurance uses configurable criteria (patient control number, claim charge amount, dates, etc.).

Logic behind the scenes is more complex than what the UI exposes.

Criteria can be enabled/disabled per customer, but defaults apply first.

Matching Behavior:

Rule of thumb: “If we don’t know, we don’t match.” → Avoids false matches.

Alternate matching methodologies exist but require looking into code for exact logic.

Manual Linking:

Users can manually match/unmatch ERA to claims in the portal (payer activity page).

Manual linking/unlinking is UI-driven, though APIs exist internally.

Multiple Matching:

Generally one ERA record links to one claim.

Multiple ERAs can link to one claim, but multiple claims per ERA record isn’t a typical use case.

Provider-level adjustments (PLB segments) are not claim-linked, they stay at check/global level.

When No Claim Found:

ERA processing continues regardless.

The 835 is stored in CID, delivered downstream, and can still generate posting files.

Claim matching failure does not block ERA flow.

2. RPA ERA–Claim Linking
How It Works

ERA File Structure:

File from Exchange → one envelope (ISA + GS).

Multiple ST segments → each represents a check.

Each ST has multiple CLPs → each represents a claim payment.

Splitting Process:

File is split at ST level, then further split into CLP units.

Each CLP becomes an independent external unit (with ISA, GS, ST + unique CLP).

Enrollment Check:

Before processing claims, RPA checks enrollment (Tax ID, NPI, payer ID).

If enrollment doesn’t exist → reject check early.

Claim Matching Logic:

Matching starts at CLP level (always claim-based).

Steps:

Use payer ID + patient account number (CLP01) to find possible claims.

Validate using REF 6R (unique reference key).

Cross-check Tax ID + NPI to avoid mismatches.

If multiple matches, filter with date of service, charge amount, procedure code, sub-service ID, REF 6R, acceptance status, client ID, and time constraints (up to 2 years back).

If one or more matches → link all (no restriction).

Manual Linking:

If auto-link fails, users can go to the ERA/EOB screen → choose “Match” option.

Pre-populated filters (Tax ID, date of service, etc.) show candidate claims.

User manually selects the correct claim and links it.

When No Claim Found:

ERA is still processed and delivered.

Claim linking failure does not stop ERA processing.

Manual correction can be done later.1. Assurance ERA–Claim Linking
How It Works

Matching Criteria:

Assurance uses configurable criteria (patient control number, claim charge amount, dates, etc.).

Logic behind the scenes is more complex than what the UI exposes.

Criteria can be enabled/disabled per customer, but defaults apply first.

Matching Behavior:

Rule of thumb: “If we don’t know, we don’t match.” → Avoids false matches.

Alternate matching methodologies exist but require looking into code for exact logic.

Manual Linking:

Users can manually match/unmatch ERA to claims in the portal (payer activity page).

Manual linking/unlinking is UI-driven, though APIs exist internally.

Multiple Matching:

Generally one ERA record links to one claim.

Multiple ERAs can link to one claim, but multiple claims per ERA record isn’t a typical use case.

Provider-level adjustments (PLB segments) are not claim-linked, they stay at check/global level.

When No Claim Found:

ERA processing continues regardless.

The 835 is stored in CID, delivered downstream, and can still generate posting files.

Claim matching failure does not block ERA flow.

2. RPA ERA–Claim Linking
How It Works

ERA File Structure:

File from Exchange → one envelope (ISA + GS).

Multiple ST segments → each represents a check.

Each ST has multiple CLPs → each represents a claim payment.

Splitting Process:

File is split at ST level, then further split into CLP units.

Each CLP becomes an independent external unit (with ISA, GS, ST + unique CLP).

Enrollment Check:

Before processing claims, RPA checks enrollment (Tax ID, NPI, payer ID).

If enrollment doesn’t exist → reject check early.

Claim Matching Logic:

Matching starts at CLP level (always claim-based).

Steps:

Use payer ID + patient account number (CLP01) to find possible claims.

Validate using REF 6R (unique reference key).

Cross-check Tax ID + NPI to avoid mismatches.

If multiple matches, filter with date of service, charge amount, procedure code, sub-service ID, REF 6R, acceptance status, client ID, and time constraints (up to 2 years back).

If one or more matches → link all (no restriction).

Manual Linking:

If auto-link fails, users can go to the ERA/EOB screen → choose “Match” option.

Pre-populated filters (Tax ID, date of service, etc.) show candidate claims.

User manually selects the correct claim and links it.

When No Claim Found:

ERA is still processed and delivered.

Claim linking failure does not stop ERA processing.

Manual correction can be done later.

Multiple Linking
Multiple ERAs → 1 claim allowed. 1 ERA → multiple claims generally not allowed.
1 ERA (CLP) can link to multiple claims if matches found.

4. End-to-End Workflow Summary
Assurance

Receive ERA (835).

Parse and try matching to claims based on configurable criteria.

If match found → link.

If no match → still process ERA (store + deliver).

Users can manually match/unmatch claims in portal.

RPA

Receive ERA from Exchange (ISA → GS → ST → CLP).

Split into claim-level units (CLPs).

Perform enrollment check before processing.

For each CLP:

Attempt claim match (payer ID + patient account + REF 6R + filters).

If matches → link (possibly multiple).

If no matches → leave unmatched.

ERA still processed and delivered regardless.

Manual linking possible via ERA/EOB screen.



✅ Common Grounds (already aligned, no gap):

Manual linking → used in both projects.

Primary check = PCN → both rely on this.

Multiple claims can be linked → no blocking risk here.
