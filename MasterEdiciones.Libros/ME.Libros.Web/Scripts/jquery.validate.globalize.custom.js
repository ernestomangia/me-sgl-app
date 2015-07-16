function handleGroups(value) {
    var culture = Globalize.findClosestCulture(),
        nf = culture.numberFormat,
        intAndFraction = value;

    var integer,
		decSep = nf["."],
        groupSep = nf[","],
        neg = nf["-"],
		pos = nf["+"],
		decimalPos = intAndFraction.indexOf(decSep);

    //Subtract integer
    integer = decimalPos < 0 ? intAndFraction : intAndFraction.substr(0, decimalPos);
    //Delete sign
    integer = integer.replace(new RegExp("[" + pos + "|" + neg + "]", 'g'), '');

    var groups = integer.split(groupSep);
    if (groups.length > 1) {
        // if the digits are grouped, check if the grouping is valid.
        var i, groupLength, groupSize;
        for (i = 0; i < groups.length; i++) {

            groupLength = groups[groups.length - i - 1].length;

            if (groupLength == 0) {
                // empty groups are not allowed
                return false;
            }
            groupSize = nf.groupSizes[Math.min(i, nf.groupSizes.length - 1)];

            if (i == groups.length - 1) {
                // the last group should not be longer than the group size (unless its 0 meaning no more separators from here).
                if (groupSize > 0 && groupLength > groupSize) {
                    return false;
                }
            }
            else {
                if (groupLength != groupSize) {
                    // invalid when incorrect group size
                    return false;
                }
            }
        }
    }
    return true;
}
//function handleGroups(integer) {
//    var groupSep = nf[","];
//    var groups = integer.split(groupSep);
//    if (groups.length > 1) {
//        // if the digits are grouped, check if the grouping is valid.
//        var i, groupLength, groupSize;
//        for (i = 0; i < groups.length; i++) {

//            groupLength = groups[groups.length - i - 1].length;

//            if (groupLength == 0) {
//                // empty groups are not allowed
//                return ret;
//            }
//            groupSize = nf.groupSizes[Math.min(i, nf.groupSizes.length - 1)];

//            if (i == groups.length - 1) {
//                // the last group should not be longer than the group size (unless its 0 meaning no more separators from here).
//                if (groupSize > 0 && groupLength > groupSize) {
//                    return ret;
//                }
//            }
//            else {
//                if (groupLength != groupSize) {
//                    // invalid when incorrect group size
//                    return ret;
//                }
//            }
//        }
//    }

//    return true;
//}