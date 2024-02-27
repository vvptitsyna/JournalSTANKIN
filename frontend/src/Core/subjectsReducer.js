const initialState = {
    subject: [],
    students: [],
    groups: [],
    selectedGroup: null,
    selectedSubject: null,
    selectedRelation: null,

}

const SELECT_SUBJECT = "SELECT_SUBJECT"
const GET_SUBJECT = "GET_SUBJECT"
const GET_STUDENTS = "GET_STUDENTS"
const GET_GROUPS = "GET_GROUPS"
const SELECT_GROUP = "SELECT_GROUP"
const SELECT_RELATION = "SELECT_RELATION"
const UPDATE_STUDENT_MARK ="UPDATE_STUDENT_MARK"

const SubjectReducer = (state = initialState, action) => {
    switch (action.type) {
        case SELECT_SUBJECT:
            return {
                ...state,
                selectedSubject: action.payload,
            };
        case GET_SUBJECT:
            return {
                ...state,
                subject: action.payload,
            };
        case GET_STUDENTS:
            return {
                ...state,
                students: action.payload,
            };
        case GET_GROUPS:
            return {
                ...state,
                groups: action.payload,
            }
        case SELECT_GROUP:
            return {
                ...state,
                selectedGroup: action.payload,
            }
        case SELECT_RELATION:
            return {
                ...state,
                selectedRelation: action.payload,
            }
        case UPDATE_STUDENT_MARK:
            return {
                ...state,
                students: state.students.map(student => {
                    if (student.$id === action.payload.$id) {
                        // Обновляем только те свойства, которые изменились
                        return {
                            ...student,
                            studentName: action.payload.studentName,
                            module1: action.payload.module1,
                            module2: action.payload.module2,
                            examOrTest: action.payload.examOrTest,
                            coursework: action.payload.coursework,
                        };
                    }
                    return student;
                })
            };
        default:
            return state;
    }
}

export default SubjectReducer;
export const getSubjectAction = (payload) => ({type: GET_SUBJECT, payload});
export const getStudentsAction = (payload) => ({type: GET_STUDENTS, payload});
export const getGroupsAction = (payload) => ({type: GET_GROUPS, payload});
export const selectGroupAction = (payload) => ({type: SELECT_GROUP, payload});
export const selectSubjectAction = (payload) => ({type: SELECT_SUBJECT, payload})
export const selectRelationAction = (payload) => ({type: SELECT_RELATION, payload})
export const updateStudentMark = (payload) => ({type: UPDATE_STUDENT_MARK, payload})