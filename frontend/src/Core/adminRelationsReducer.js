const initialState = {
    relations: [],
    teachers: [],

}


const GET_RELATIONS = "GET_RELATIONS"
const GET_TEACHERS = "GET_TEACHERS"
const adminRelationsReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_RELATIONS:
            return {
                ...state,
                relations: action.payload,
            };
        case GET_TEACHERS:
            return {
                ...state,
                teachers: action.payload,
            };
        default:
            return state;
    }
}

export default adminRelationsReducer;
export const getAdminRelationsAction = (payload) => ({type: GET_RELATIONS, payload});
export const getAdminTeachersAction = (payload) => ({type: GET_TEACHERS, payload});