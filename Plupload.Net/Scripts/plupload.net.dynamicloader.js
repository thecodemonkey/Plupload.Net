function LoadPlupload() {

    LoadScriptDynamically(ResolveServerPath(_tmpPluploadContext.JSPluploadPreload, _tmpPluploadContext.ApplicationPath), LoadingPreloadScriptComplete);
}

function LoadingPreloadScriptComplete() 
{
    _pluploadContext = _tmpPluploadContext;

    document.getElementById('plp_progress_outer').style.display = 'block';
    PreloadingJQueryComplete();
}