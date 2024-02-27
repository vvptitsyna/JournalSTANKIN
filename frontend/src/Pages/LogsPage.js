import List from "../SimpleComponents/List";
import {useEffect, useState} from "react";
import API from "../Core/api";
import {getAdminGroupsAction, selectedAdminGroupAction} from "../Core/adminGroupsReducer";

function LogsPage () {

    const [logs, setLogs] = useState([]);

    useEffect(() => {
        API.get(`Administrator/getAllLogMessages`, { withCredentials: true })
            .then(res => {
                setLogs(res.data.$values);
                console.log(res.data.$values)
            }).catch(res => {
            console.log(res.error);
        })
    },[]);

    return (
        <div>
            <List children = {logs} fieldsToShow={['message']} fieldsHeader={['Лог']}/>
        </div>
    );
}
export default LogsPage;