import react from "react";
import {combineReducers, createStore} from "redux";
import semestersReducer from "./semestersReducer";
import subjectsReducer from "./subjectsReducer";
import { persistReducer, persistStore } from 'redux-persist';
import storage from 'redux-persist/lib/storage';
import usersReducer from "./usersReducer";
import adminSubjectReducer from "./adminSubjectsReducer";
import adminSemestersReducer from "./adminSemestersReducer";
import adminGroupsReducer from "./adminGroupsReducer";
import adminRelationsReducer from "./adminRelationsReducer";

const rootReducer = combineReducers({
    sem: semestersReducer,
    subj: subjectsReducer,
    users: usersReducer,
    adminSubjects: adminSubjectReducer,
    adminSemesters: adminSemestersReducer,
    adminGroups: adminGroupsReducer,
    adminRelations: adminRelationsReducer,
})


const persistConfig = {
    key: 'root',
    storage,
};

const persistedReducer = persistReducer(persistConfig, rootReducer);
export const store = createStore(persistedReducer)
export const persistor = persistStore(store);