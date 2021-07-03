var config = {
    mode: "fixed_servers",
    rules: {
        singleProxy: {
            scheme: "http",
            host: "%HOST%",
            port: parseInt(%PORT%)
        },
        bypassList: ["localhost"]
    }
};

chrome.proxy.settings.set({ value: config, scope: "regular" }, function () { });

var username = "%USERNAME%";
var password = "%PASSWORD%";
var retry = 3;

function callbackFn(details) {
    return {
        authCredentials: {
            username: username ,
            password: password 
        }
    };
}

chrome.webRequest.onAuthRequired.addListener(
            callbackFn,
            {urls: ["<all_urls>"]},
            ['blocking']
);