import React, { useContext, useEffect, useState } from 'react'
import { createStyles, makeStyles, Avatar, Box, Typography, } from '@material-ui/core'
import { DataGrid } from '@mui/x-data-grid';

import AddPerson from '../../crud/AddPerson/AddPerson'
import UserContext from '../../../context'
import UpdatePerson from '../../crud/UpdatePerson/UpdatePerson'
import DeletePerson from '../../crud/DeletePerson/DeletePerson'

const useStyles = makeStyles((theme) => createStyles({
	container: {
		height: '700px',
		width: '100%',
	},
	header: {
		display: 'flex',
		justifyContent: 'space-between',
		margin: theme.spacing(2, 0),
	},
	text: {
		margin: theme.spacing(1),
	},
}));

function PersonManagement() {
	const classes = useStyles();
	const { user, _ } = useContext(UserContext);
	const [loading, setLoading] = useState(true);
	const [persons, setPersons] = useState([]);
	const [total, setTotal] = useState(0);
	const [page, setPage] = useState(1);
	const [onPage, setOnPage] = useState(10);
	const [reload, setReload] = useState(false);

	useEffect(() => {
		fetch(`https://localhost:5001/api/Person?PageNumber=${page}&OnPageCount=${onPage}`)
			.then(response => response.json())
			.then(result => {
				setPersons(result.items);
				setTotal(result.totalCount);
				setLoading(false);
				setReload(false);
			})
			.catch(_ => _);
	}, [page, onPage, reload]);

	const columns = [
		{
			field: 'image',
			headerName: 'Фото',
			renderCell: (params) => (
				<Avatar
					src={`https://localhost:5001/${params.value.photoPath ? params.value.photoPath : "Unknown.png"}`}
					alt={params.value.fullName}
				/>
			),
			align: 'center',
			filterable: false,
			headerAlign: 'center',
			sortable: false,
			width: 100,
		},
		{
			field: 'fullName',
			headerName: 'ФИО',
			align: 'left',
			filterable: false,
			headerAlign: 'center',
			sortable: false,
			width: 900,
		},
		{
			field: 'update',
			headerName: 'Изменить',
			renderCell: (params) => (
				<UpdatePerson
					id={params.value.id}
					fullName={params.value.fullName}
					onUpdate={handleReload}
				/>
			),
			align: 'center',
			filterable: false,
			headerAlign: 'center',
			sortable: false,
			width: 100,
		},
		{
			field: 'delete',
			headerName: 'Удалить',
			renderCell: (params) => (
				<DeletePerson id={params.value.id} onDelete={handleReload} />
			),
			align: 'center',
			filterable: false,
			headerAlign: 'center',
			sortable: false,
			width: 100,
		},
	];

	const getRows = () => {
		const arr = [];
		persons.forEach(item => {
			arr.push({
				id: item.id,
				fullName: item.fullName,
				image: { photoPath: item.photoPath, fullName: item.fullName },
				update: { id: item.id, fullName: item.fullName },
				delete: { id: item.id },
			});
		});
		return arr;
	}

	const handlePageChange = (page, _) => {
		setPage(page + 1);
		setLoading(true);
	}

	const handleOnPageChange = (pageSize, _) => {
		setOnPage(pageSize);
		setLoading(true);
	}

	const handleReload = () => {
		setReload(true);
	}

	return (
		<>
			{
				user.isAdmin
					? <>
						<Box className={classes.header}>
							<Typography variant='h4' className={classes.text}>Знаменитости</Typography>
							<AddPerson onCreate={handleReload} />
						</Box>
						<Box className={classes.container}>
							<DataGrid
								rows={getRows()}
								columns={columns}
								page={page - 1}
								onPageChange={handlePageChange}
								pageSize={onPage}
								onPageSizeChange={handleOnPageChange}
								rowsPerPageOptions={[10, 20, 50]}
								rowCount={total}
								loading={loading}
								paginationMode='server'
								disableColumnMenu
								disableSelectionOnClick
								style={{ fontSize: 'medium', }}
							/>
						</Box>
					</>
					: <Typography variant='subtitle1'>Для просмотра страницы необходимо войти как администратор</Typography>
			}

		</>
	)
}

export default PersonManagement