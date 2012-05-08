/*
    this script is a template. It contains placeholders, wich will be resolved on the server.
  
    {0} - custom StartUpFunction
*/

var _alreadyStarted = false;
var _loadJQuery = true;

//onload function, preloades an external/embedded initial script.
//wich contains a progressbar                 
function StartUp() {                   
    if (!_alreadyStarted)         
        _alreadyStarted = true;
    else              
        return;

    LoadScriptDynamically('[SCRIPT_JQUERY_URL]', LoadingJQUERYScriptComplete);
}

function LoadingJQUERYScriptComplete() {
    LoadScriptDynamically('[SCRIPT_URL]', [ON_COMPLETE_FUNCTION]);
}

function LoadScriptDynamically(scriptURL, onCompleteFunction, onErrorFunction)
{
    if (IsScriptAllreadyLoaded(scriptURL)) {
        onCompleteFunction();
        return;
    }
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.OnComplete = onCompleteFunction;

    if (typeof(onErrorFunction) != 'undefined' || typeof(onErrorLoadScriptDynamically) != 'undefined'){
        script.onerror = (typeof(onErrorFunction) != 'undefined')? onErrorFunction : onErrorLoadScriptDynamically;
    }

    if (script.readyState) {  //IE
        script.onreadystatechange = function () {
            if (this.readyState == "loaded" || this.readyState == "complete") {
                this.onreadystatechange = null;
                this.OnComplete();
            }
        };
    } else {  //Others
        script.onload = function () {
            this.OnComplete();
        };
    }

    script.src = scriptURL;
    document.getElementsByTagName("head")[0].appendChild(script);    
}


function IsScriptAllreadyLoaded(src) {
    var scripts = document.getElementsByTagName("script");
    if (typeof (scripts) != 'undefined' && scripts != null && scripts.length > 0) {
        for (var x = 0; x < scripts.length; x++) {
            if (scripts[x].src.toLowerCase() == src.toLowerCase()) return true;
        }
    }

    return false;
}

function HasResource(resource) {                                   
    return (typeof (resource) != 'undefined' && resource != '' && resource != ' ' && resource != 'null');
}

function ResolveServerPath(url, applicationPath) {
    return url.replace("~/", applicationPath);
}

//shows errors while loading script dynamically
function StartUpLoadingError(err) {
    alert('error:' + err);
}

//dynamically registration of the onlload function
function initReady(fn) {
    if (document.addEventListener) {
        document.addEventListener("DOMContentLoaded", fn, false);
    }
    else {
        document.onreadystatechange = function () { readyState(fn) }
    }
}

function readyState(func) {
    if (document.readyState == "interactive" || document.readyState == "complete") {
        func();
    }
}

//loads styles and scripts automatically
function RegisterScriptLoading(load) {
    if (load) {
        window.onDomReady = initReady;
        window.onDomReady(StartUp);
    }
}  