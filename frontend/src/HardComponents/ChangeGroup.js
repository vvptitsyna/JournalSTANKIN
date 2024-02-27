import {useDispatch, useSelector} from "react-redux";
import API from "../Core/api";
import {getSubjectsAction} from "../Core/semestersReducer";
import {selectSubjectAction} from "../Core/subjectsReducer";
import {useEffect, useState} from "react";
import {setUserAction} from "../Core/usersReducer";
import Input from "../SimpleComponents/Input";
import Select from 'react-select'
import Button from "../SimpleComponents/Button";

const ChangeGroup = ({
                        onUserChanged, ...attrs
                    }) => {

    const dispatch = useDispatch();
    const group = useSelector(state => state.adminGroups.version)
    const selectedGroup = useSelector(state => state.adminGroups.selectedVersion)

    const [groupData, setGroupData] = useState({
        groupWithVersionId: selectedGroup,
        name: group.name,
        comment: group.comment
    });

    useEffect(() => {
        setGroupData({
            groupWithVersionId: selectedGroup,
            name: group.name,
            comment: group.comment,
        });
    }, [group])

    const handleUserDataChange = (e) => {
        setGroupData({
            ...groupData,
            [e.target.id]: e.target.value
        })}

    const handleSubmit = (e) => {
        e.preventDefault();
        API.post('Administrator/Group/editGroupWithVersion', groupData, { withCredentials: true } )
            .then(res => {
                console.log (groupData);
                console.log (res.data);
                onUserChanged();
            }) .catch(res => {
            console.log(res.error);
            console.log (res);
        })
        console.log(group);
    }
    return (
        <div className="wrapper-change">
            <form className="change-form" onSubmit={e => handleSubmit(e)}>
                <h1>Изменить группу</h1>
                <Input id="name" value={groupData.name} onChange={e => handleUserDataChange(e)} type="text" className="add"/>
                <Input id="comment" value={groupData.comment} onChange={e => handleUserDataChange(e)} type="text" className="add"/>
                <Button children="Изменить" type="submit"/>
            </form>
        </div>

    );
};

export default ChangeGroup;