const initialState = {
    groups: [],
    selectedGroup: null,
    versions: [],
    selectedVersion: null,
    version: [],
    selectedSubGroup: null,
    subGroups: [],
    subGroup: [],
    students: [],
}


const GET_GROUPS = "GET_GROUPS"
const SELECT_GROUP = "SELECT_GROUP"
const GET_VERSIONS = "GET_VERSIONS"
const SELECT_VERSION = "SELECT_VERSION"
const GET_VERSION = "GET_VERSION"
const SELECT_SUBGROUP = "SELECT_SUBGROUP"
const GET_SUBGROUPS = "GET_SUBGROUPS"
const GET_SUBGROUP = "GET_SUBGROUP"
const GET_STUDENTS = "GET_STUDENTS"

const adminGroupsReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_GROUPS:
            return {
                ...state,
                groups: action.payload,
            };
        case SELECT_GROUP:
            return {
                ...state,
                selectedGroup: action.payload,
            };
        case GET_VERSIONS:
            return {
                ...state,
                versions: action.payload,
            };
        case SELECT_VERSION:
            return {
                ...state,
                selectedVersion: action.payload,
            };
        case GET_VERSION:
            return {
                ...state,
                version: action.payload,
            };
        case SELECT_SUBGROUP:
            return {
                ...state,
                selectedSubGroup: action.payload,
            };
        case GET_SUBGROUPS:
            return {
                ...state,
                subGroups: action.payload,
            };
        case GET_SUBGROUP:
            return {
                ...state,
                subGroup: action.payload,
            };
        case GET_STUDENTS:
            return {
                ...state,
                students: action.payload,
            };
        default:
            return state;
    }
}

export default adminGroupsReducer;
export const getAdminGroupsAction = (payload) => ({type: GET_GROUPS, payload});
export const selectedAdminGroupAction = (payload) => ({type: SELECT_GROUP, payload});
export const getAdminVersionsAction = (payload) => ({type: GET_VERSIONS, payload});
export const selectedAdminGroupVersionAction = (payload) => ({type: SELECT_VERSION, payload});
export const getAdminVersionAction = (payload) => ({type: GET_VERSION, payload});
export const selectedAdminSubGroupAction = (payload) => ({type: SELECT_SUBGROUP, payload});
export const getAdminSubGroupsAction = (payload) => ({type: GET_SUBGROUPS, payload});
export const getAdminSubGroupAction = (payload) => ({type: GET_SUBGROUP, payload});
export const getAdminSubgroupStudentsAction = (payload) => ({type: GET_STUDENTS, payload});