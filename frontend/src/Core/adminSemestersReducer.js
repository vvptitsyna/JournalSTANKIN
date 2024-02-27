const initialState = {
    semesters: [],

}


const GET_SEMESTERS = "GET_SEMESTERS"

const adminSemestersReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_SEMESTERS:
            return {
                ...state,
                semesters: action.payload,
            };
        default:
            return state;
    }
}

export default adminSemestersReducer;
export const getAdminSemestersAction = (payload) => ({type: GET_SEMESTERS, payload});