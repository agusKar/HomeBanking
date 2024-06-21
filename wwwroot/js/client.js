var app = new Vue({
    el: "#app",
    data: {
        clientInfo: {},
        isAdmin: false,
        clients: [],
        clientById: {},
        //error: null
        errorToats: null,
        errorMsg: null,
    },
    methods: {
        getData: function () {
            axios.get("/api/clients/current")
                .then(function (response) {
                    //get client ifo
                    app.clientInfo = response.data;

                    app.clientInfo.email == "agustin@gmail.com" ? app.isAdmin = true : app.isAdmin = false;
                })
                .catch(function (error) {
                    // handle error
                    //app.error = error;
                    this.errorMsg = "Error getting data";
                    this.errorToats.show();
                })
        },
        getAllClient: function () {
            axios.get("/api/clients")
                .then(function (response) {
                    console.log("CLIENTES", response.data);
                    app.clients = response.data;
                })
                .catch(function (error) {
                    // handle error
                    //app.error = error;
                    this.errorMsg = "Error getting data";
                    this.errorToats.show();
                })
        },
        getClientById: function (id) {
            axios.get(`/api/clients/${id}`)
                .then(function (response) {
                    app.clientById = response.data;
                    this.modal = new bootstrap.Modal(document.getElementById('seeClient'));
                    this.modal.show();
                })
                .catch(function (error) {
                    // handle error
                    //app.error = error;
                    this.errorMsg = "Error getting data";
                    this.errorToats.show();
                })
        },
        signOut: function () {
            axios.post('/api/auth/logout')
                .then(response => window.location.href = "/index.html")
                .catch(() => {
                    this.errorMsg = "Sign out failed"
                    this.errorToats.show();
                })
        },
        create: function () {
            axios.post('/api/clients/current/accounts')
                .then(response => window.location.reload())
                .catch((error) => {
                    this.errorMsg = error.response.data;
                    this.errorToats.show();
                })
        }
    },
    mounted: function () {
        this.errorToats = new bootstrap.Toast(document.getElementById('danger-toast'));
        this.getData();
        this.getAllClient();
    }
})