﻿Globalize.load({
    "main": {
        "en": {
            "identity": {
                "version": {
                    "_cldrVersion": "25",
                    "_number": "$Revision: 91 $"
                },
                "generation": {
                    "_date": "$Date: 2014-03-13 22:27:12 -0500 (Thu, 13 Mar 2014) $"
                },
                "language": "en"
            },
            "dates": {
                "calendars": {
                    "gregorian": {
                        "months": {
                            "format": {
                                "abbreviated": {
                                    "1": "Jan",
                                    "2": "Feb",
                                    "3": "Mar",
                                    "4": "Apr",
                                    "5": "May",
                                    "6": "Jun",
                                    "7": "Jul",
                                    "8": "Aug",
                                    "9": "Sep",
                                    "10": "Oct",
                                    "11": "Nov",
                                    "12": "Dec"
                                }
                            }
                        },
                        "dayPeriods": {
                            "format": {
                                "wide": {
                                    "am": "AM",
                                    "am-alt-variant": "am",
                                    "noon": "noon",
                                    "pm": "PM",
                                    "pm-alt-variant": "pm"
                                }
                            }
                        },
                        "dateFormats": {
                            "medium": "MMM d, y"
                        },
                        "timeFormats": {
                            "medium": "h:mm:ss a",
                        },
                        "dateTimeFormats": {
                            "medium": "{1}, {0}"
                        }
                    }
                },
                "fields": {
                    "second": {
                        "displayName": "Second",
                        "relative-type-0": "now",
                        "relativeTime-type-future": {
                            "relativeTimePattern-count-one": "in {0} second",
                            "relativeTimePattern-count-other": "in {0} seconds"
                        },
                        "relativeTime-type-past": {
                            "relativeTimePattern-count-one": "{0} second ago",
                            "relativeTimePattern-count-other": "{0} seconds ago"
                        }
                    }
                }
            },
            "numbers": {
                "currencies": {
                    "USD": {
                        "symbol": "$"
                    }
                },
                "defaultNumberingSystem": "latn",
                "symbols-numberSystem-latn": {
                    "decimal": ",",
                    "exponential": "E",
                    "group": ".",
                    "infinity": "∞",
                    "minusSign": "-",
                    "nan": "NaN",
                    "percentSign": "%",
                    "perMille": "‰",
                    "plusSign": "+",
                    "timeSeparator": ":"
                },
                "decimalFormats-numberSystem-latn": {
                    "standard": "#,##0.###"
                },
                "currencyFormats-numberSystem-latn": {
                    "currencySpacing": {
                        "beforeCurrency": {
                            "currencyMatch": "[:^S:]",
                            "surroundingMatch": "[:digit:]",
                            "insertBetween": " "
                        },
                        "afterCurrency": {
                            "currencyMatch": "[:^S:]",
                            "surroundingMatch": "[:digit:]",
                            "insertBetween": " "
                        }
                    },
                    "standard": "¤#,##0.00"
                }
            }
        }
    },
    "supplemental": {
        "version": {
            "_cldrVersion": "25",
            "_number": "$Revision: 91 $"
        },
        "currencyData": {
            "fractions": {
                "DEFAULT": {
                    "_rounding": "0",
                    "_digits": "2"
                }
            }
        },
        "likelySubtags": {
            "en": "en-Latn-US",
        },
        "plurals-type-cardinal": {
            "en": {
                "pluralRule-count-one": "i = 1 and v = 0 @integer 1",
                "pluralRule-count-other": " @integer 0, 2~16, 100, 1000, 10000, 100000, 1000000, … @decimal 0.0~1.5, 10.0, 100.0, 1000.0, 10000.0, 100000.0, 1000000.0, …"
            }
        }
    }
});

//(function (window, undefined) {

//    var Globalize;

//    if (typeof require !== "undefined" &&
//        typeof exports !== "undefined" &&
//        typeof module !== "undefined") {
//        // Assume CommonJS
//        Globalize = require("globalize");
//    } else {
//        // Global variable
//        Globalize = window.Globalize;
//    }

//    Globalize.load({
//        name: "es-AR",
//        englishName: "Spanish (Argentina)",
//        nativeName: "Español (Argentina)",
//        language: "es",
//        numberFormat: {
//            ",": ".",
//            ".": ",",
//            "NaN": "NeuN",
//            negativeInfinity: "-Infinito",
//            positiveInfinity: "Infinito",
//            percent: {
//                ",": ".",
//                ".": ","
//            },
//            currency: {
//                pattern: ["$-n", "$ n"],
//                ",": ".",
//                ".": ","
//            }
//        },
//        calendars: {
//            standard: {
//                days: {
//                    names: ["domingo", "lunes", "martes", "miércoles", "jueves", "viernes", "sábado"],
//                    namesAbbr: ["dom", "lun", "mar", "mié", "jue", "vie", "sáb"],
//                    namesShort: ["do", "lu", "ma", "mi", "ju", "vi", "sá"]
//                },
//                months: {
//                    names: ["enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", ""],
//                    namesAbbr: ["ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", ""]
//                },
//                AM: ["a.m.", "a.m.", "A.M."],
//                PM: ["p.m.", "p.m.", "P.M."],
//                eras: [{ "name": "d.C.", "start": null, "offset": 0 }],
//                patterns: {
//                    d: "dd/MM/yyyy",
//                    D: "dddd, dd' de 'MMMM' de 'yyyy",
//                    t: "hh:mm tt",
//                    T: "hh:mm:ss tt",
//                    f: "dddd, dd' de 'MMMM' de 'yyyy hh:mm tt",
//                    F: "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt",
//                    M: "dd MMMM",
//                    Y: "MMMM' de 'yyyy"
//                }
//            }
//        }
//    });

//}(this));


//Globalize.load({
//    main: {
//        es: {
    
//        }
//    },
//    supplemental: {
//        likelySubtags: {
    
//        },
//        timeDate: {
    
//        },
//        weekData: {
    
//        }
//    }
//});