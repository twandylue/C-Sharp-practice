using MVC.Models;

namespace MVC.Respository
{
    public interface ISSOAccount
    {
        (bool, string) BindAccount(sso_account_binding bindRelationShip);
        (bool, string) LoginSSOAccount(StateInfo stateInfo, string sourceId);
    }
}