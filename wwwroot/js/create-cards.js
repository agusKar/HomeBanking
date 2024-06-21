var app = new Vue({
    el:"#app",
    data: {
        clientInfo: {},
        errorToats: null,
        errorMsg: null,
        cardType: "none",
        isAdmin: false,
        cardColor:"none",
    },
    methods:{
        formatDate: function(date){
            return new Date(date).toLocaleDateString('en-gb');
        },
        GetDataClient: function () {
            //axios.get("/api/clients/1")
            axios.get("/api/clients/current")
                .then(function (response) {
                    app.clientInfo = response.data;
                    app.clientInfo.email == "agustin@gmail.com" ? app.isAdmin = true : app.isAdmin = false;
                })
                .catch(function (error) {
                    // handle error
                    this.errorMsg = "Error getting data";
                    this.errorToats.show();
                })
        },
        signOut: function(){
            axios.post('/api/auth/logout')
            .then(response => window.location.href="/index.html")
            .catch(() =>{
                this.errorMsg = "Sign out failed"   
                this.errorToats.show();
            })
        },
        create: function(event){
            event.preventDefault();
            if(this.cardType == "none" || this.cardColor == "none"){
                this.errorMsg = "You must select a card type and color";  
                this.errorToats.show();
            }else{
/*                 let config = {
                    headers: {
                        'content-type': 'application/x-www-form-urlencoded'
                    }
                }
                axios.post(`http://localhost:8080/api/clients/current/cards?cardType=${this.cardType}&cardColor=${this.cardColor}`,config)
                .then(response => window.location.href = "/web/cards.html")
                .catch((error) =>{
                    this.errorMsg = error.response.data;  
                    this.errorToats.show();
                }) */
                axios.post('/api/clients/current/cards',{
                    type: this.cardType,
                    color: this.cardColor,
                })
                    .then(() => { window.location.href = "/cards.html" })
                    .catch((error) => {
                        console.log(error)
                        this.errorMsg = error.response.data
                        this.errorToats.show();
                    })
            }
        }
    },
    mounted: function(){
        this.errorToats = new bootstrap.Toast(document.getElementById('danger-toast'));
        this.GetDataClient();
    }
})