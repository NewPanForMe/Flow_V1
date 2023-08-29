const api = {
    user: {
        GetUserList: window.config.baseUrl + "/User/GetList",
        Add: window.config.baseUrl + "/User/Add",
        Edit: window.config.baseUrl + "/User/Update",
        Delete: window.config.baseUrl + "/User/Delete",
        GetEntityByCode: window.config.baseUrl + "/User/GetEntityByCode",
        BindPhone: window.config.baseUrl + "/User/BindPhone",
        EditPassword: window.config.baseUrl + "/User/EditPassword",
        
    },
    module: {
        GetModuleList: window.config.baseUrl + "/Module/GetList",
        Add: window.config.baseUrl + "/Module/Add",
        Edit: window.config.baseUrl + "/Module/Update",
        Delete: window.config.baseUrl + "/Module/Delete",
        GetEntityByCode: window.config.baseUrl + "/Module/GetEntityByCode",
        GetGroupSelectOptions: window.config.baseUrl + "/Module/GetGroupSelectOptions",
        GetSelectOptions: window.config.baseUrl + "/Module/GetSelectOptions",

    },
    login: {
        checkUserName: window.config.baseUrl + "/Login/Check",
    },
    refreshToken:{
        RefreshToken: window.config.baseUrl + "/RefreshToken/RefreshToken",
    },
    tree:{
        GetModuleTreeNode: window.config.baseUrl + "/Tree/ModuleTree",
    },
    menu:{
        GetMenu: window.config.baseUrl + "/Menu/Menu",
    },
    log:{
        GetLogList:window.config.baseUrl + "/NLog/GetLogList",
    },
    role: {
        GetRoleList: window.config.baseUrl + "/Role/GetList",
        Add: window.config.baseUrl + "/Role/Add",
        Edit: window.config.baseUrl + "/Role/Update",
        Delete: window.config.baseUrl + "/Role/Delete",
        GetEntityByCode: window.config.baseUrl + "/Role/GetEntityByCode",
        GetRoleOptions:window.config.baseUrl + "/Role/GetRoleOptions",
    },
    sms:{
        SendBindCode:window.config.baseUrl + "/Sms/SendBindCode",
        SendRegisterCode:window.config.baseUrl + "/Sms/SendRegisterCode",
        GetSmsList: window.config.baseUrl + "/Sms/GetList",
    },
    file:{
        FileUpload: window.config.baseUrl + "/File/Upload",
        GetFileList: window.config.baseUrl + "/File/GetList",
        GetHasUploadList: window.config.baseUrl + "/File/GetHasUploadList",
        Delete: window.config.baseUrl + "/File/Delete",
    }
}
export default api