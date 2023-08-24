const api = {
    bill: {
        BillUpload: window.config.baseUrl + "/Bill/BillUpload",
        GetList: window.config.baseUrl + "/Bill/GetList",
        GetBillEntityByCode: window.config.baseUrl + "/Bill/GetBillEntityByCode",
        
    },
    billDetail:{
        GetList: window.config.baseUrl + "/BillDetail/GetList",
        GetBillDetailEntityByCode: window.config.baseUrl + "/BillDetail/GetBillDetailEntityByCode",
    },
    chart:{
        GetTypeChart: window.config.baseUrl + "/Chart/GetTypeChart",
        GetPayTypeChart: window.config.baseUrl + "/Chart/GetPayTypeChart",
        GetInChart: window.config.baseUrl + "/Chart/GetInChart",
    }
}
export default api