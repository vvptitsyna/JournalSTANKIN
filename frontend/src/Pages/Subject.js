import Navigation from "../HardComponents/Navigation";
import SubjectInfo from "../Components/SubjectInfo";
import ButtonGroup from "../SimpleComponents/ButtonGroup";
import Button from "../SimpleComponents/Button";
import {useDispatch, useSelector} from "react-redux";
import React, {useEffect} from "react";
import API from "../Core/api";
import {getSubjectsAction, setSemestersAction} from "../Core/semestersReducer";
import {
    getGroupsAction,
    getJournalAction,
    getStudentsAction,
    getSubjectAction,
    selectRelationAction
} from "../Core/subjectsReducer";
import Table from "../SimpleComponents/Table";


function Subject () {
    const dispatch = useDispatch();
    const subject = useSelector(state => state.subj.subject);
    const groups = useSelector(state => state.subj.groups);
    const students = useSelector(state => state.subj.students);
    const selectedGroup = useSelector(state => state.subj.selectedGroup);
    const subjectId = useSelector(state => state.subj.selectedSubject)
    const relationId = useSelector(state => state.subj.selectedRelation)
    const semesterId = useSelector(state => state.sem.selectedSemester)

    useEffect(() => {
        API.get(`Teacher/getGroupsInfo?semesterId=${semesterId}&subjectId=${subjectId}`, { withCredentials: true })
            .then(res => {
                dispatch(getGroupsAction(res.data.$values))
                dispatch(selectRelationAction(res.data.$values[0].relationId))
            })
        API.get(`Teacher/getRelationInfo?relationId=${relationId}`, { withCredentials: true })
            .then(res => {
                dispatch(getStudentsAction(res.data.marks.$values))
                dispatch(getSubjectAction(res.data))
            })
    }, [dispatch])
    const handleSelectGroup = (id) => {
        API.get(`Teacher/getSemesterInfo?semesterId=${id}`, { withCredentials: true })
            .then(res => {
                dispatch(getStudentsAction(res.data.$values))
                console.log(res.data)
            })
    }
    return (
        <div>
            <Navigation />
            <SubjectInfo />
            <ButtonGroup>
                             {groups.map((group) => {
                                 return (
                                     <Button onClick={() => handleSelectGroup(group.id)} className="semester-btn" id={group.$id} children={group.groupName + ' ' + group.subgroupName} />
                                 )
                             })
                             }
            </ButtonGroup>
            <Table/>
        </div>
    );
}

export default Subject;