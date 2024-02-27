const initialState = {
subjects: [],

}


const GET_SUBJECTS = "GET_SUBJECTS"

const adminSubjectReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_SUBJECTS:
            return {
                ...state,
                subjects: action.payload,
            };
        default:
            return state;
    }
}

export default adminSubjectReducer;
export const getAdminSubjectsAction = (payload) => ({type: GET_SUBJECTS, payload});