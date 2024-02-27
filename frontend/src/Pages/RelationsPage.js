import {useDispatch, useSelector} from "react-redux";
import {useEffect, useState} from "react";
import API from "../Core/api";
import {getAdminSemestersAction} from "../Core/adminSemestersReducer";
import {getAdminRelationsAction} from "../Core/adminRelationsReducer";
import List from "../SimpleComponents/List";
import AddRelation from "../HardComponents/AddRelation";

function RelationsPage () {

    const dispatch = useDispatch();
    const relations = useSelector(store => store.adminRelations.relations)
    const [refreshRelations, setRefreshRelations] = useState(false);

    useEffect(() => {
        API.get(`/Administrator/Relation/getAdminRelationsInfo`, { withCredentials: true })
            .then(res => {
                dispatch(getAdminRelationsAction(res.data.$values))
                console.log(res.data.$values)
                console.log(relations)
            }).catch(res => {
            console.log(res.error);
        })
    }, [refreshRelations]);

    const handleRefreshRelations = () => {
        setRefreshRelations(!refreshRelations);
        console.log('Я ОБНОВИЛАСЬ')
    };

    return (
        <div className="wrapper-relations">
            <AddRelation onUserChanged={handleRefreshRelations}/>
            <div className="wrapper-list">
                <h1>Список cвязей:</h1>
                <List children = {relations} fieldsToShow={['subjectName', 'groupName', 'subgroupName', 'semesterName']} fieldsHeader={['Предмет', 'Группа','Подгруппа','Семестр']}/>
            </div>
        </div>
    );
}
export default RelationsPage;