

const extractTokens = function (uri) {

    const returnValue = uri.split('#')[1];
    const values = returnValue.split('&');

    for (var i = 0; i < values.length; i++) {
        let v = values[i];

        let kvp = v.split('=');

        localStorage.setItem(kvp[0], kvp[1]);
    }

    location.href = '/home/index';
}

extractTokens(location.href);
