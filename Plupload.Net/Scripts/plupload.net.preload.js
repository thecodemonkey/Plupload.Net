var _pluploadContext = null;


/* Preloading Plupload.Net */

//preloads all scripts and styles       
function PreloadingJQueryComplete() {
    var preloadingContext2 = new PreloadingContext(OnAfterPreloading);

    if (HasResource(_pluploadContext.JSBrowserPlus))
        preloadingContext2.AddScript("JSBrowserPlus", "browser plus handling", null, OnReceiveError, ResolveServerPath(_pluploadContext.JSBrowserPlus));

    if (HasResource(_pluploadContext.JSPluploadFull))
        preloadingContext2.AddScript("JSPluploadFull", "plupload core library", null, OnReceiveError, ResolveServerPath(_pluploadContext.JSPluploadFull));

    if (HasResource(_pluploadContext.JSPluploadQueue))
        preloadingContext2.AddScript("JSPluploadQueue", "plupload queue", null, OnReceiveError, ResolveServerPath(_pluploadContext.JSPluploadQueue));

    if (HasResource(_pluploadContext.JSPluploadDotNet))
        preloadingContext2.AddScript("JSPluploadDotNet", "plupload dot net client component", null, OnReceiveError, ResolveServerPath(_pluploadContext.JSPluploadDotNet));

    if (HasResource(_pluploadContext.JSLightbox))
        preloadingContext2.AddScript("JSLightbox", "lightbox library", null, OnReceiveError, ResolveServerPath(_pluploadContext.JSLightbox));

    if (HasResource(_pluploadContext.CSSPluploadQueue))
        preloadingContext2.AddStylesheet("pluploadqueue", "plupload queue styles", null, OnReceiveError, ResolveServerPath(_pluploadContext.CSSPluploadQueue));

    if (HasResource(_pluploadContext.CSSLightBox))
        preloadingContext2.AddStylesheet("lightbox", "lightbx styles", null, OnReceiveError, ResolveServerPath(_pluploadContext.CSSLightBox));

    if (HasResource(_pluploadContext.CSSPluploadDotNet))
        preloadingContext2.AddStylesheet("pluploadDotNet", "plupload .net ui customizing", null, OnReceiveError, ResolveServerPath(_pluploadContext.CSSPluploadDotNet));

    preloadingContext2.Load();
}

function HasResource(resource) {
    return (typeof (resource) != 'undefined' && resource != '' && resource != ' ' && resource != 'null');
}

function OnReceiveError(response, textStatus, errorThrown) {
    alert("resp: " + response + " | textStatus: " + textStatus + " | errorThrown: " + errorThrown);
}

function OnAfterPreloading() {

    document.getElementById('plp_progress_outer').style.display = 'none';

    InitializePlupload();
}


function InitializePlupload() {
    if (_pluploadContext.AutoInit) {
        $("#uploader").pluploadQueue({
            runtimes: _pluploadContext.Runtimes,
            url: ResolveServerPath(_pluploadContext.PluploadServerURL),
            max_file_size: _pluploadContext.MaxFileSize,
            multiple_queues: _pluploadContext.MultipleQueues,
            urlstream_upload: _pluploadContext.URLStreamUpload,

            resize: (_pluploadContext.Resize != null) ? _pluploadContext.Resize : [],

            filters: (_pluploadContext.FileFilters != null) ? _pluploadContext.FileFilters : [],
            flash_swf_url: ResolveServerPath(_pluploadContext.Flash),
            silverlight_xap_url: ResolveServerPath(_pluploadContext.Silverlight)
        });

        PLInitialize();
    }
}


function ResolveServerPath(url) {
    return url.replace("~/", _pluploadContext.ApplicationPath);
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

/* END preloading Plupload.Net */


/* common preloading stuff */   
function PreloadingContext(oncomplete) {
    this.Items = new Array();               
    this.PreloadingCompleteFunction = oncomplete;
}                                                       

function PreloadingItem(name, description, oncomplete, onerror) {
    this.IsPreloadingComplete = false;
    this.PreloadingCompleteFunction = oncomplete;
    this.PreloadingErroredFunction = onerror;
    this.Name = name;
    this.Description = description;
    this.GetType = function() { return typeof (this); }
    this.Context = null;
}

function ImageItem(name, description, oncomplete, onerror, imageURL) 
{                                                   
    PreloadingItem.apply(this, arguments);      
    this.ImageURL = imageURL;                       
    this.LoadItem = function() {                
        var img = new Image();          
        img.onload = this.OnComplete;
        img.onerror = this.OnError;
        img.onabort = this.OnError;
        
        img.Context = this.Context;
        img.CurentItem = this;
        
        img.src = this.ImageURL;
    }
}

function ScriptItem(name, description, oncomplete, onerror, src) {
    if (IsScriptAllreadyLoaded(src)) {
        oncomplete();
        return;
    }


    PreloadingItem.apply(this, arguments);
    this.Src = src;
    this.LoadItem = function() {
        var script = document.createElement("script")
        script.type = "text/javascript";
        script.OnComplete = this.OnComplete;
        script.onerror = this.OnError;

        script.Context = this.Context;
        script.CurentItem = this;

        if (script.readyState) {  //IE
            script.onreadystatechange = function() {
                if (this.readyState == "loaded" ||
                    this.readyState == "complete") {
                    this.onreadystatechange = null;
                    this.OnComplete();
                }
            };
        } else {  //Others
            script.onload = function() {
                this.OnComplete();
            };
        }

        script.src = this.Src;
        document.getElementsByTagName("head")[0].appendChild(script);
    }
}

function StylesheetItem(name, description, oncomplete, onerror, href) 
{
    PreloadingItem.apply(this, arguments);
    this.Href = href;
    this.LoadItem = function() {
        var client = new XMLHttpRequest();

        client.OnComplete = this.OnComplete;
        client.OnError = this.OnError;
        client.Context = this.Context;
        client.CurentItem = this;

        client.onreadystatechange = function() {
            if (this.readyState == 4 && this.status == 200) {
                createStyleTag(this.responseText);
                this.OnComplete();
            } else if (this.readyState == 4 && this.status != 200) {
                this.OnError(this, this.statusText, null);
            }
        };

        client.open("GET", this.Href);
        client.send();
    }
}

function createStyleTag(styleContent)
{
    var styletag = document.createElement("style");
    styletag.setAttribute('type', 'text/css');

    document.getElementsByTagName("head")[0].appendChild(styletag);
    
    if (!window.ActiveXObject) {
        try
        {
            styletag.innerHTML = styleContent;
        }catch(exp)
        {
            styletag.innerText = styleContent; //Safari       
        }
    }
    else {
        styletag.styleSheet.cssText = styleContent; // if Internet Explorer
    }
}

ScriptItem.prototype = PreloadingItem.prototype;
StylesheetItem.prototype = PreloadingItem.prototype;
ImageItem.prototype = PreloadingItem.prototype;

PreloadingContext.prototype.AddImage = function(name, description, oncomplete, onerror, imageurl) {
    var ii = new ImageItem(name, description, oncomplete, onerror, imageurl);
    ii.Context = this;
    this.Items.push(ii);
}

PreloadingContext.prototype.AddStylesheet = function(name, description, oncomplete, onerror, href) {
    var si = new StylesheetItem(name, description, oncomplete, onerror, href);
    si.Context = this;
    this.Items.push(si);
}

PreloadingContext.prototype.AddScript = function(name, description, oncomplete, onerror, src) {
    var si = new ScriptItem(name, description, oncomplete, onerror, src);
    si.Context = this;
    this.Items.push(si);
}


PreloadingContext.prototype.Load = function() {
    var x = 0;
                
    for (x = 0; x < this.Items.length; x++) 
    {
        this.Items[x].LoadItem();
    }
}


PreloadingItem.prototype.OnComplete = function(response) {
    this.CurentItem.IsPreloadingComplete = true;
    if (typeof (this.CurentItem.PreloadingCompleteFunction) != 'undefined' && this.CurentItem.PreloadingCompleteFunction != null) {
        this.CurentItem.PreloadingCompleteFunction(response);
    }
    this.Context.UpdateProgress(this.Context, this.CurentItem);
    this.Context.CompletePreloading();
}

PreloadingItem.prototype.OnError = function(response, textStatus, errorThrown) {
    this.CurentItem.IsPreloadingComplete = true;
    if (typeof (this.CurentItem.PreloadingErroredFunction) != 'undefined' && this.CurentItem.PreloadingErroredFunction != null) {
        this.CurentItem.PreloadingErroredFunction(response, textStatus, errorThrown);
    }
    this.Context.UpdateProgress(this.Context, this.CurentItem);
    this.Context.CompletePreloading();
}

PreloadingContext.prototype.IsPreloadingComplete = function() 
{
    var isPreloadingComplete = true;

    var x = 0;
    for (x = 0; x < this.Items.length; x++) {
        if (!this.Items[x].IsPreloadingComplete) {
            isPreloadingComplete = false;
            break;
        }
    }

    return isPreloadingComplete;
}

PreloadingContext.prototype.CompletePreloading = function() {
    if (this.IsPreloadingComplete()) {
        this.PreloadingCompleteFunction();
    }
}

PreloadingContext.prototype.GetLoadState = function() {
    var nLoadedItemsCount = 0;

    for (var x = 0; x < this.Items.length; x++) 
    {
        if (this.Items[x].IsPreloadingComplete) nLoadedItemsCount++;
    }

    return Math.round((100 / this.Items.length * nLoadedItemsCount));
}

PreloadingContext.prototype.UpdateProgress = function (context, preloadingItem) {
    var progressLable = document.getElementById('plp_progress_lable');
    var progressState = document.getElementById('plp_progress_done');

    var percentLoaded = (context.IsPreloadingComplete()) ? 100 : context.GetLoadState();

    if (typeof (progressLable) != 'undefined') {
        var progress = '{0}% loaded. {1} - {2}';
        progressLable.innerHTML = progress.replace('{0}', percentLoaded).replace('{1}', preloadingItem.Name).replace('{2}', preloadingItem.Description);
    }

    if (typeof (progressState) != 'undefined') {
        progressState.style.width = percentLoaded + " %";
    }
}