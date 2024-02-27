const initialState = {
    users: [],
    selectedUser: null,
    selectedUserForChange: []
}

const GET_USERS = "GET_USERS"
const SET_USER = "SET_USER"
const SELECT_USER = "SELECT_USER"

const usersReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_USERS:
            return {
                ...state,
                users: action.payload,
            };
        case SET_USER:
            return {
                ...state,
                user: action.payload,
            };
        case SELECT_USER:
            return {
                ...state,
                selectedUser: action.payload
            }
        default:
            return state;
    }
}

export default usersReducer;
export const getUsersAction = (payload) => ({type: GET_USERS, payload});
export const setUserAction = (payload) => ({type: SET_USER, payload});
export const selectUserAction = (payload) => ({type: SELECT_USER, payload})