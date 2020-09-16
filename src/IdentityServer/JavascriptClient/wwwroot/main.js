const config = {
    userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
    authority: "https://localhost:5000",
    client_id: "client_id_js",
    //response_type: "id_token token",
    response_type: "code",
    post_logout_redirect_uri: "https://localhost:5004/home/index",
    redirect_uri: "https://localhost:5004/home/signin",
    scope: "openid ApiOne rc.scope ApiTwo"
}

const userManager = new Oidc.UserManager(config);

const signIn = function () {

    userManager.signinRedirect();
}

const signOut = function () {
    userManager.signoutRedirect();
}

userManager.getUser()
    .then(user => {

        if (user) {
            axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token;
        }
    })

const callApi = function () {
    axios.get("http://localhost:5001/api/secret")
        .then(res => {
            console.log(res);
        })
}

let refreshing = false;

axios.interceptors.response.use(res => res, error => {
    console.log(error.response);

    const axiosConfig = error.response.config;

    // if error is unauthorized -> try refresh token
    if (error.response.status === 401) {

        if (!refreshing) {
            refreshing = true;

            return userManager.signinSilent()
                .then(res => {
                    axios.defaults.headers.common["Authorization"] = "Bearer " + res.access_token;
                    axiosConfig.headers["Authorization"] = "Bearer " + res.access_token;
                    refreshing = false;
                    return axios(axiosConfig);
                }).catch(x => {
                    console.log(x);
                    refreshing = false;
                    return Promise.reject(x);
                })

        }
    }

    return Promise.reject(error);
})