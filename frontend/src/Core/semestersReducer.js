const initialState = {
    semesters: [],
    selectedSemester: null,
    subjects: [],
}

const GET_SUBJECTS = "GET_SUBJECTS"
const SET_SEMESTERS = "SET_SEMESTERS"
const SELECT_SEMESTER = "SELECT_SEMESTER"

const SemesterReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_SUBJECTS:
            return {
                ...state,
                subjects: action.payload,
            };
        case SET_SEMESTERS:
            return {
                ...state,
                semesters: action.payload,
            };
        case SELECT_SEMESTER:
            return {
                ...state,
                selectedSemester: action.payload
            }
        default:
            return state;
    }
}

export default SemesterReducer;
export const getSubjectsAction = (payload) => ({type: GET_SUBJECTS, payload});
export const setSemestersAction = (payload) => ({type: SET_SEMESTERS, payload});
export const selectSemesterAction = (payload) => ({type: SELECT_SEMESTER, payload})
