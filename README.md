Assurance call notes on ERA to claim Linking.

Vaidyanathan, Raj 2 minutes 43 seconds
Perfect. Perfect. Thank you. All right. So that done we can I have 4-5 other topics we can go over. One of the things was linking to the claim and we we did notice that the criteria for linking to a claim is.
Pretty, you know, not detailed in in Assurance compared to RPA. RPA uses like 10 different things. Supriya, correct me if I'm wrong.

Hawn, Timothy A
Assurance does as well. It has a number of things and they can all be configured to match on certain things. Assurance also the Assurance user has the ability to manually match or remit to a claim.

Vaidyanathan, Raj
OK.

Hawn, Timothy A
Or unmatch or remit if it happened to match incorrectly. Assurance. Typically our our rule of thumb is we don't guess if we don't know, we don't match. So there's a number of alternate matching methodologies, what those entail, the exact fields they look at.
That will be something that Dave or someone will probably need to go look in the code, see exactly what's happening behind the scenes as what we present to the user is pretty.

Vaidyanathan, Raj
OK.

Hawn, Timothy A
I don't know. I I I don't want to say generic, but it it's simplified. We may say we're gonna match on patient control number, but there may be a bunch of logic behind the scenes there, so.

Vaidyanathan, Raj
What we've seen so far, again, we've seen only a little bit is the patient control number and the claim charge amount, right, Supriya, that's what we saw.

AS
Acharya, Supriya
Yeah.

Vaidyanathan, Raj
Those two were the only ones we have seen so far.

Hawn, Timothy A
Yeah, dates would be in there. There's there's other. Let me see if I can pull up assurance while we go here and show where those claim options are.

Vaidyanathan, Raj
You can, yeah, we'll definitely be interested in that. But we don't have customer specific logic for that, right? I mean it's for everybody, same logic or are we saying that it could differ?

Hawn, Timothy A
It can be different by customer. It's not hard coded by customer. It's options that customers can enable and disable to change that behavior.

Vaidyanathan, Raj
Interesting. OK, I guess we'll for RPA, we'll have a single behavior for everybody 'cause they're all used to that, I guess and.

Hawn, Timothy A
Well, and to be honest with you, all all customers start out with the default, whatever the code default is, but it can further be modified from.

Vaidyanathan, Raj
OK, OK. And when you said you can manually match or unmatch, that is by going onto the Assurance portal, pick something and then say either match it or unmatch it, right? There's no API or anything behind the scenes, right?

Hawn, Timothy A
uh some API behind. There it goes. There's some API internal. It's all within the product that the customer through the UI on the payer activity page.

Vaidyanathan, Raj
OK, perfect. One more question on the linking. Is it possible to have an ERA linked to multiple claims or the other way around multiple ERAS linking to a single claim?

Hawn, Timothy A
Well, you know, honestly, I don't know. I don't know that we would ever have a use case where a claim level 835 record would be applicable to attach to more than one claim in assurance. You know what I mean?

Vaidyanathan, Raj
yes

Hawn, Timothy A
So when we're when we're linking, when we're linking the ERA, we're talking about linking a specific record in an 835 to a claim, not the claim file or the check to to that remit.
or to that claim, if that makes sense.

Vaidyanathan, Raj
Yeah, yeah, because we in RPA we do have both possible can have multiple claims to an ERA, multiple ERA to a claim depending on the service lines.

Hawn, Timothy A
Yeah, you can. You can definitely have multiple ERAS to claim, but there would never be a case that I can think of where I would have a check payment for.
Dave Frisbie claim that would be applicable to more than one Dave Frisbie claim ever, because it is that remit would have been generated off of a single claim getting to payer, you know what I mean? So that.
I I I we may be talking about different use cases here. If you're talking about PLB segments being applied to multiple claims, Assurance doesn't really apply Pl. BS to a claim.
The PLB is that would be something like an interest payment or or something global to the check as opposed to being unique to a claim. So anyway.

Vaidyanathan, Raj
That's the the provider adjustment, right?

Hawn, Timothy A
Right, right.

Vaidyanathan, Raj
Be'll be OK.
OK, uh, let's see.
Any anything else team? Anybody has anything on the linking 'cause we have at least four or five different topics to go over.
LS
Likhitha, Singamaneni
Yes, Raj. So when no claims found for an ERA, are we still processing it in assurance like RPA?

Vaidyanathan, Raj
I think what she asked, correct me if I'm wrong, that if the claim is not found, it doesn't affect the behavior or the processing of the ERA itself. We can still process, right?

Hawn, Timothy A
Not at all. Yeah, not at all. The it it if we we can have and we often do have 830 fives come to a CID and no claims match for whatever reason.
The 835 can still be delivered. It is still stored in the CID, so a posting file could be created or whatever, and it's also passed on to auto agent to be delivered as well, so.
We when the 835 comes in, we see if we we parse it out, we see if we can match it to something in the system. If we can't, we don't and it moves on. It doesn't interrupt processing or throw errors or anything like that.

Vaidyanathan, Raj
OK, perfect. Yeah, that is matching with RPA, you know, because the claim may or may not have gone through RPA. So we don't really stop the processing of the area for that. OK, that makes sense.

	RPA call notes on ERA to claim Linking.
Vaidyanathan, Raj

All right, so a little background on how ERA is structured in RP at least. So we get a file from Exchange. Exchange gets it from the payer, right? So a file that comes from Exchange has a single envelope. It has one ISA, it has one GS.

And it has, you know, bunch of STS. Each St. represents one check, and within that one check it can have a whole bunch of CLPS. The CLPS represent claim payments.


Yeah, this is the one. OK, so a file checks. Within the checks we have multiple CLPS. So the way RPA processes ERA is takes the file, it splits at the St. level. So it takes an St. and then it splits further into CLPS.
So if a check had 10 CLPs, we create 10 individual external units, each one containing its own ISA, GS, St. We copy all the check information, but we keep just the CLP information unique. So before we start processing the entire check, we have to make sure that.
That enrollment is existing for that particular check. So what we do is we take the first CLP or the last CLP, one or the other, and we say, hey, make sure that this tax ID, this NPI and this payer ID has an enrollment on RPA before you start processing everything.
So we we send that as a a a check transaction. Um.
If it comes back and says no, there is no enrollment for this particular combination, we do not process that check anymore. We'll just simply reject it and say there is no enrollment. There's no point going further. If it comes back saying it is successful, yes, we want an enrollment and also.
There's no duplicate for it. Then we further send all the CLPS one at a time. When we process CLP one at a time, that's where the claim matching comes into play because the claim is always linked at the CLP level.
So we do a couple of things. We have a, you know, alternate there. We have a main way to look for a claim. If that doesn't work out, we have an alternate way to look at it. The first thing we want to do is make sure that the.
The the payer ID, right? We take the payer ID from the ERA and we take some the patient account number which is the CLP 01 and we say for this combination.
Tell me, do you have any claims at all? OK, and the payer ID can be more than one, at least in RPA. A payer can have a different set of payer IDs for claims and a different set of payer IDs for ERA, the same payer.
United Healthcare may have like 5 different payer IDs for claims and five different payer IDs for ERA. So when we get an ERA we say first of all get me all the possible claim IDs #1 claim payer IDs and then we say for that list of claim payer IDs and this CLP 01 are there any?
Claims in RPM and this is where we use that PTLS lookup. This is a service we use. If it comes back as no not found. At that point we stop. Yeah, there's nothing to, you know, there's nothing to do further. We could not find anything. If it comes back, there's at least one or more came back.
We then take take the REF 6R in the CLP which is of the service level and say hey do you have the REF 6R matching? The reason we do that is because we generate a REF 6R that is unique. So if the payer sent it we can get we can guarantee yeah this is our this is our guide.
So that's our first first attempt. We put the RDF 6R. We also check for the tax ID in the NPI, just so we don't want to, you know, send it to a wrong person. If these things match, then we find the claim, we link it, it's all good. If it doesn't match, let's say yes, we found a bunch of them, but this doesn't match.
Then we do the difficult work of looking for all these other things. OK, you found claims. Now do a filter. So let's say you got 100 claims. We say we need the one where the data service matches with the ERA, the charged amount, the procedure code, sub service ID, the REF 6R which will not match.
And the claim also has to be accepted. We only can do ERA when the claim is accepted. So if the claim was rejected, you will never find it. And we also make sure the client ID matches because this is a PHI thing, right? If the claim came from a client ID A, you want to make sure the ERA is also going to the same client ID.
And then we last we check the time amount if all these things match.
And let's say we get one or more, we will link all of them. It's usually one. For the most part, we don't get more than one, but we don't have the limitation or restriction that we can only limit one. You know, if you get more than one, link it all. So it goes through the the rigorous.
You know, search of these guys, but this is our number one preferred way to do it. Now Assurance, I can't remember, there were only two items. They were looking for the claim charge, I think, and the patient account number.
I believe.
AS
Acharya, Supriya

That's what I remember too.

Vaidyanathan, Raj

Right. So that that won't work for us because especially in lower environments, when we reprocess the file over and over again, we are repeating the same patient account number, we're repeating the same details. We're not even considering the process date. I think in our case we go up to.
I think I wanna say two years. We only go back up to two years claims. It used to be 180 days. I think they extended it to two years and last year, a couple of years ago or so.
So that is how the claim matching works. We so we need to document kind of what we do and then what Assurance does and see if there is any risk of doing that way. Obviously nobody's complaining on Assurance. They must be doing something right, but we need to, you know, bring that up.
Any questions on this at all?
AS
Acharya, Supriya

Just wanted to add Raj that this is automatic, right? Like coming from the system. If for whatever reason if an ERA does not get linked to any claim on the portal, we also have a functionality to do manual.

Vaidyanathan, Raj

Right. That's, that's, that's right. That's a good call. So yeah, so let's say when we processed the ERA, let's say for whatever reason some of the enrollment was not set up or something went wrong and we could not link it to a claim, we processed the ERA. That's the other thing.
AS
Acharya, Supriya

Linking.

Vaidyanathan, Raj

ERA not matching to a claim does not mean anything for the processing of the ERA. We will go ahead and process the ERA and deliver it matching. If matching doesn't work, it doesn't mean that we'll reject the check. No, we'll simply process it because there are legitimate cases.
Where you can process an ERA and not have a claim because the claim may not have gone through our system, right? So.
That is valid scenario. Like Supriya said, if a client comes back and says, hey, this was supposed to match to this claim, it didn't happen. So either the client or the support team or somebody will do the necessary research to figure out this is the claim and this is the ERA that they don't, that they belong together, then they can pick up.
Do you link from the claim standpoint or the rest standpoint, Supriya?
AS
Acharya, Supriya

Flames standpoint.

Vaidyanathan, Raj

OK, so you go to the claim and you say now link it to the CRA and you can manually link it.
AS
Acharya, Supriya

Oh no, no, no. Sorry. Yeah, I got your question on that. No, no, no, no. You go to the ERA screen right on the EOB. There is if if if we figure that that particular ERA does did not have any claim matched, then there is a link. There is an option there. It says match or something like that.

Vaidyanathan, Raj

Right.
AS
Acharya, Supriya

There's a button so when you click on that it opens up a window and it has pre populated tax ID, date of service, bunch of information that is already pre populated and it'll give you a bunch of claims that you know that fits that criteria. Then it's up to the client to see which.

Vaidyanathan, Raj

Mhm.
AS
Acharya, Supriya

One you know they think is the is the the exact name that they are looking for to match.

Vaidyanathan, Raj

Right, so they do all the that work and link it. It doesn't go through this logic, it's simply just you point out this is the claim, this is the ERA, go and link it and it works. So we need to see I I thought I saw a feature where you can link a claim to an ERA in assurance. We just have to make sure that that is existing.
That that is a great feature we have.
