function Login() {
    var url = "https://accounts.google.com/o/oauth2/v2/auth?";
    url += "scope=email profile&";
    url += "redirect_uri=https://localhost:5001/&";
    url += "response_type=code&";
    url += "client_id=536062935773-e1hvscne4ead0kk62fho999kc179rhhj.apps.googleusercontent.com&";
    url += "state=12345&";
    url += "approval_prompt=force&";
    window.location.href = url;
}
// Login()
const button = document.querySelector("#LoginButton")
button.addEventListener("click", ()=> {Login()});

const GetAccounts = async () => {
    const res = await fetch("/api/v1/accounts", {
        method: "GET",
        headers: new Headers({
            "Content-Type": "application/json",
        })
    })
    const accounts = await res.json()
    console.log(accounts);
}
// GetAccounts();
