const LoginButton = document.querySelector("#LoginButton")
LoginButton.addEventListener("click", ()=> {Login()});

const SignButton = document.querySelector('#SignupButton')

const Login = async () => {
    const res = await fetch("api/v1/LoginSSO", {
        method: "GET",
        headers: new Headers({
            "Content-Type": "application/json",
        })
    })
    const {redirect_Uri} = await res.json()
    window.location.href = redirect_Uri
}

const sendUrl = async () => {
    const urlSearchParams = new URLSearchParams(window.location.search)
    const params = Object.fromEntries(urlSearchParams.entries());
    if (params.code !== undefined || params.state !== undefined) {
        const url = `/api/v1/checkPortalSSO?code=${params.code}&state=${params.state}` 
        const res = await fetch(url, {
            method: "GET",
            headers: new Headers({
                "Content-Type": "application/json",
            })
        })
        const {message} = await res.json()
        if (res.status === 200) {
            Swal.fire({
                icon: "success",
                title: "登入成功",
                text: message,
                showCancelButton: true,
                confirmButtonText: "確定",
                cancelButtonText: "取消"
            })
        } else {
            Swal.fire({
                icon: "error",
                title: "登入失敗",
                text: message,
                confirmButtonText: "確定",
                cancelButtonText: "取消"
            })
        }
    }
    
}
sendUrl();