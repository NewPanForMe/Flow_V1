import allApi from './home/index'
import billApi from './bill/index'
const api = {
    ...allApi,
    ...billApi,
}
export default api