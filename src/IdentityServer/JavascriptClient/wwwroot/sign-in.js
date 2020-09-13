const createState = function () {
    return 'VeryLongStateValue12312312312312dasfsfsdfasdfawefasdf';
}

const createNonce = function () {
    return 'VeryLongNonceValuefa3wr3wfrv3awrva3wrva3wrv3warvaw3rvw3';
}

const signIn = function () {

    let redirectUri = 'https://localhost:5004/home/signin'
    let responseType = 'id_token token'
    let scope = 'openid ApiOne'

    let authUrl =
        `/connect/authorize/callback?client_id=client_id_js&redirect_uri=${encodeURIComponent(redirectUri)}&response_type=${encodeURIComponent(responseType)}&scope=${encodeURIComponent(scope)}&nonce=${createNonce()}&state=${createState()}`

    let returnUrl = encodeURIComponent(authUrl)

    window.location.href = `https://localhost:5000/auth/login?ReturnUrl=${returnUrl}`;
}