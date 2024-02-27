import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Auth from './Pages/Authorization';
import Main from "./Pages/Main";
import {Provider} from "react-redux";
import {store, persistor} from "./Core/store";
import Subject from "./Pages/Subject";
import {PersistGate} from "redux-persist/integration/react";
import Administration from "./Pages/Administration";
import AdminPage from "./Pages/AdminPage";
import SupportMain from "./Pages/SupportMain";
import EditVersionPage from "./Pages/EditVersionsPage";
import EditSubGroupsPage from "./Pages/EditSubGroupsPage";

const authPage = ReactDOM.createRoot(document.getElementById('wrapper'));

authPage.render(
    <Provider store={store}>
        <PersistGate loading={null} persistor={persistor}>
            <BrowserRouter>
                <Routes>
                    <Route path='/main' element={<Main />} />
                    <Route path='/' element={<Auth />} exact/>
                    <Route path='/subject' element={<Subject />}/>
                    <Route path='/administration' element={<Administration />}/>
                    <Route path="/admin" element={<AdminPage />} />
                    <Route path="/support" element={<SupportMain />} />
                    <Route path="/edit-version" element={<EditVersionPage/>} />
                    <Route path="/edit-subgroup" element={<EditSubGroupsPage/>} />
                </Routes>
            </BrowserRouter>
        </PersistGate>
    </Provider>
);