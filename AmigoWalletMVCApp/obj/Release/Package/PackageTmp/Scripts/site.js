$(function () {
    window.page = new phed.Presenters.BasePage();
    page.init();
    /*********************************** POPUP SCRIPT goes here ...........*********************/
    var present = new Date().getFullYear();
    $("#current").html(present);
    $(".blackoverlay, .popup_div").hide();
});

var phed = { "Presenters": {} };

phed.Presenters.BasePage = function (options) {
    var defaults = {
        eventHandlers: {}
    };
    this.settings = $.extend(defaults, options);
    this.APIMethods = {
        GET: "GET",
        PUT: "PUT",
        POST: "POST",
        DELETE: "DELETE"
    };
};
phed.Presenters.BasePage.prototype = {
    init: function () {

    },
    /*** progress ******************************************************/
    showProgress: function () {
        if ($("#divProgressIndicator") != undefined) {
          //  $("#divProgressIndicator").dialog("destroy");
            $("#divProgressIndicator").dialog({
                autoOpen: true,
                modal: true,
                height: 120,
                width: 305,
                resizable: false,
                closeOnEscape: false,
                dialogClass: 'no-close'
            });
        }
    },
    hideProgress: function () {
        if ($("#divProgressIndicator").data("ui-dialog")) {
            $("#divProgressIndicator").dialog("destroy");
        }
    },
    isDate: function (txtDate) {
        var currVal = txtDate;
        if (currVal == '') {
            return false;
        }
        //Declare Regex 
        var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
        var dtArray = currVal.match(rxDatePattern); // is format OK?
        if (dtArray == null) {
            return false;
        }
        dtMonth = dtArray[1];
        dtDay = dtArray[3];
        dtYear = dtArray[5];  
if (dtMonth < 1 || dtMonth > 12)
    return false;
else if (dtDay < 1 || dtDay> 31)
    return false;
else if ((dtMonth==4 || dtMonth==6 || dtMonth==9 || dtMonth==11) && dtDay ==31)
    return false;
else if (dtMonth == 2) {
    var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
    if (dtDay > 29 || (dtDay == 29 && !isleap))
        return false;
    }
    return true;

    },
    ServerRequest: function (servicePath, callbackMethod, errorCallbackMethod, callbackData, requestData, timeout, methodType) {
        var _this = this;
        if (typeof requestData == 'undefined')
            requestData = {};
        if (typeof callbackMethod == 'undefined'
			|| callbackMethod == null)
            callbackMethod = function (data) { _this.ExecuteCallBack(callbackData, data); };

        if (typeof timeout == 'undefined' || timeout == null)
            timeout = 0;
        var url = window.base_url + servicePath;
        var type = methodType; //_this.APIMethods.POST;
        var dataType = "json";

        $.ajax({
            type: type,
            url: url,
            data: JSON.stringify(requestData),
            contentType: "application/json; charset=utf-8",
            dataType: dataType,
            success: callbackMethod,
            error: function (xhr, desc, exceptionobj) { _this.ServerErrorParser(xhr, desc, exceptionobj, errorCallbackMethod) },
            callbackData: callbackData
        });
    },

    dateFilter: function dateFilter(k, v) {
        return (typeof v == "string"
  && (k = v.match(/([0-9]{4})-([0-9]{2})-([0-9]{2})T([0-9]{2}):([0-9]{2}):([0-9]{2})Z$/))) ? new Date(Date.UTC(k[1], k[2] - 1, k[3], k[4], k[5], k[6])) : v;
    },
    isHandledException: function (errors) {
        //TODO - need to clean this list..
        //all specific exceptions can be clubbed to ValidationException - unless there is a case for specific ones.
        if (errors.FaultType == "ValidationFault" || (errors.ExceptionType && errors.ExceptionType.indexOf('ValidationException') > 0)
		|| (errors.Error && errors.Error.Type && errors.Error.Type.indexOf('BadAdsException') > 0)
		|| (errors.Error && errors.Error.Type && errors.Error.Type.indexOf('BadFileException') > 0)
		|| (errors.Error && errors.Error.Type && errors.Error.Type.indexOf('ValidationException') > 0))
            return true;
    },

    isCallerHandlingValidationErrors: function (errorCallBack) {
        return !(typeof errorCallBack == 'undefined' || errorCallBack == null);
    },
    ServerRequestError: function (data, errors) {
    },
    ServerErrorParser: function (xhr, desc, exceptionobj, errorCallBack) {
        var errors = this.ParseServerError(xhr);
        var isCallerHandlingValidationErrors = this.isCallerHandlingValidationErrors(errorCallBack);
        if (isCallerHandlingValidationErrors && this.isHandledException(errors))
            errorCallBack(this.data, errors);
        else
            this.ServerRequestError(null, errors);
    },
    ParseServerError: function (xhr) {
        var responseText = xhr.responseText;

        if (responseText.endsWith('null')) //null is appeneded to the error json string when the method return type is an object
            responseText = responseText.substr(0, responseText.length - 4);

        if (responseText.endsWith('-1')) //-1 is appeneded to the error json string when the method return type is int
            responseText = responseText.substr(0, responseText.length - 2);

        if (responseText.indexOf("{\"d\":null}") >= 0)
            responseText = responseText.replace("{\"d\":null}", "");
        if (responseText.indexOf("{\"d\":-1}") >= 0)
            responseText = responseText.replace("{\"d\":-1}", "");
        var response = {};
        try {
            response = JSON.parse(responseText);
        }
        catch (error) {
            response = { Error: { errorCode: -1, message: 'unknown.error'} };
        }
        response.status = xhr.status;
        return response;
    },

    GetErrorMessage: function (errorObject) {
        var message = errorObject.Error.Message;

        try {
            var errors = eval((message.charAt(0) == "{" ? '[' + message + ']' : message));
            return errors;
        } catch (ex) { }

        return message;
    },

    ExecuteCallBack: function (callbackData, data) {
        if (callbackData == null || callbackData == undefined)
            return;
        if (typeof callbackData.callBack == "object") {
            callbackData.callBack.method.apply(callbackData.callBack.context, [data]);
        } else if (typeof callbackData.callBack == "function") {
            callbackData.callBack(data);
        }
    }
};
(function ($) {
    $.widget("ui.combobox", {
        _create: function () {
            var self = this,
					select = this.element.hide(),
					selected = select.children(":selected"),
					value = selected.val() ? selected.text() : "";
            var input = this.input = $("<input>")
					.insertAfter(select)
					.val(value)
					.autocomplete({
					    delay: 0,
					    minLength: 0,
					    source: function (request, response) {
					        var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
					        response(select.children("option").map(function () {
					            var text = $(this).text();
					            if (this.value && (!request.term || matcher.test(text)))
					                return {
					                    label: text.replace(
											new RegExp(
												"(?![^&;]+;)(?!<[^<>]*)(" +
												$.ui.autocomplete.escapeRegex(request.term) +
												")(?![^<>]*>)(?![^&;]+;)", "gi"
											), "<strong>$1</strong>"),
					                    value: text,
					                    option: this
					                };
					        }));
					    },
					    select: function (event, ui) {
					        ui.item.option.selected = true;
					        self._trigger("selected", event, {
					            item: ui.item.option
					        });
					        select.trigger("change");
					    },
					    change: function (event, ui) {
					        if (!ui.item) {
					            var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
									valid = false;
					            select.children("option").each(function () {
					                if ($(this).text().match(matcher)) {
					                    this.selected = valid = true;
					                    return false;
					                }
					            });
					            if (!valid) {
					                // remove invalid value, as it didn't match anything
					                $(this).val("");
					                select.val("");
					                input.data("autocomplete").term = "";
					                return false;
					            }
					        }
					    }
					})
					.addClass("ui-widget ui-widget-content ui-corner-left");

            input.data("autocomplete")._renderItem = function (ul, item) {
                return $("<li></li>")
						.data("item.autocomplete", item)
						.append("<a>" + item.label + "</a>")
						.appendTo(ul);
            };

            this.button = $("<button type='button'>&nbsp;</button>")
					.attr("tabIndex", -1)
					.attr("title", "Show All Items")
					.insertAfter(input)
					.button({
					    icons: {
					        primary: "ui-icon-triangle-1-s"
					    },
					    text: false
					})
					.removeClass("ui-corner-all")
					.addClass("ui-corner-right ui-button-icon ui-autocomplete-arrow")
					.click(function () {
					    // close if already visible
					    if (input.autocomplete("widget").is(":visible")) {
					        input.autocomplete("close");
					        return;
					    }

					    // work around a bug (likely same cause as #5265)
					    $(this).blur();

					    // pass empty string as value to search for, displaying all results
					    input.autocomplete("search", "");
					    input.focus();
					});
        },

        destroy: function () {
            this.input.remove();
            this.button.remove();
            this.element.show();
            $.Widget.prototype.destroy.call(this);
        }
    });




})(jQuery);


function getURLParameter(name) {
    return decodeURI(
        (RegExp(name + '=' + '(.+?)(&|$)').exec(location.search) || [, null])[1]
    );
}

function addParameterToLink(param, elem) {
    var _url = $(elem).attr("href");
    _url += (_url.split('?')[1] ? '&' : '?') + param;
    //console.log(_url);
    $(elem).attr("href", _url);
}

function addParameterToURL(param) {
    _url = location.href;
    _url += (_url.split('?')[1] ? '&' : '?') + param;
    return _url;
}

function setCookie(c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;
}

function getCookie(c_name) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_name) {
            return unescape(y);
        }
    }
}

function eraseCookie(name) {
    setCookie(name, "", -1);
}








