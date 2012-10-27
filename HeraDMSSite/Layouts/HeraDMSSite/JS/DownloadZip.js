/*
下载文件（夹）为Zip文件
*/
function enable() {
    var items = SP.ListOperation.Selection.getSelectedItems();
    var itemCount = CountDictionary(items);
    return (itemCount > 0);

}

function downloadZip() {

    var context = SP.ClientContext.get_current();
    this.site = context.get_site();
    this.web = context.get_web();
    context.load(this.site);
    context.load(this.web);
    context.executeQueryAsync(
        Function.createDelegate(this, this.onQuerySucceeded),
        Function.createDelegate(this, this.onQueryFailed)
    );
}

function onQuerySucceeded() {

    var items = SP.ListOperation.Selection.getSelectedItems();
    var itemCount = CountDictionary(items);

    if (itemCount == 0) return;

    var ids = "";
    for (var i = 0; i < itemCount; i++) {
        ids += items[i].id + ";";
    }

    //send a request to the zip aspx page.  
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", this.site.get_url() + this.web.get_serverRelativeUrl() + "/_layouts/heradmssite/pages/downloadzip.aspx");

    var hfSourceUrl = document.createElement("input");
    hfSourceUrl.setAttribute("type", "hidden");
    hfSourceUrl.setAttribute("name", "sourceUrl");
    hfSourceUrl.setAttribute("value", location.href);
    form.appendChild(hfSourceUrl);

    var hfItemIds = document.createElement("input")
    hfItemIds.setAttribute("type", "hidden");
    hfItemIds.setAttribute("name", "itemIDs");
    hfItemIds.setAttribute("value", ids);
    form.appendChild(hfItemIds);

    document.body.appendChild(form);
    form.submit();
}

function onQueryFailed(sender, args) {
    this.statusID = SP.UI.Status.addStatus("Download as Zip:",
        "Downloading Failed: " + args.get_message() + " <a href='#' onclick='javascript:closeStatus();return false;'>Close</a>.", true);
    SP.UI.Status.setStatusPriColor(this.statusID, "red");
}

function closeStatus() {
    SP.UI.Status.removeStatus(this.statusID);
}