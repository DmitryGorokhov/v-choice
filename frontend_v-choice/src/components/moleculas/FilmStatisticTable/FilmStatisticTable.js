import React from 'react';
import { DataGrid } from '@mui/x-data-grid';
import { createStyles, makeStyles, Box, Typography } from '@material-ui/core';
import { Link } from 'react-router-dom';

const useStyles = makeStyles((theme) => createStyles({
	container: {
		height: '600px',
		width: '100%'
	},
	text: {
		margin: theme.spacing(1),
	},
}));

function FilmStatisticTable(props) {
	const classes = useStyles();

	const columns = [
		{
			field: 'link',
			headerName: 'Название',
			renderCell: (params) => (
				<Link to={`/film/${params.value.id}`}>
					{params.value.title}
				</Link>
			),
			align: 'left',
			filterable: false,
			headerAlign: 'left',
			sortable: false,
			width: 628,
		},
		{
			field: 'requested',
			headerName: 'Просмотров',
			filterIndex: 0,
			align: 'center',
			filterable: false,
			headerAlign: 'center',
			sortable: false,
			type: 'number',
			width: 120,
		},
		{
			field: 'rate',
			headerName: 'Рейтинг',
			filterIndex: 1,
			align: 'center',
			filterable: false,
			headerAlign: 'center',
			sortable: false,
			type: 'number',
			width: 82,
		},
		{
			field: 'countRate',
			headerName: 'Оценок',
			filterIndex: 2,
			align: 'center',
			filterable: false,
			headerAlign: 'center',
			sortable: false,
			type: 'number',
			width: 77,
		},
		{
			field: 'countComment',
			headerName: 'Комментариев',
			filterIndex: 3,
			align: 'center',
			filterable: false,
			headerAlign: 'center',
			sortable: false,
			type: 'number',
			width: 135,
		},
		{
			field: 'countFavorite',
			headerName: 'В Избранном',
			filterIndex: 4,
			align: 'center',
			filterable: false,
			headerAlign: 'center',
			sortable: false,
			type: 'number',
			width: 122,
		},
	];

	const getRows = () => {
		const arr = [];
		props.data.forEach(item => {
			arr.push({
				id: item.id,
				link: { id: item.id, title: item.title },
				requested: item.requested,
				rate: item.avRate,
				countRate: item.countRate,
				countComment: item.countComment,
				countFavorite: item.countFavorite,
			});
		});
		return arr;
	}

	const handlePageChange = (page, _) => {
		props.setParams({ ...props.params, page: page + 1 });
		props.setLoading(true);
	}

	const handleOnPageChange = (pageSize, _) => {
		props.setParams({ ...props.params, page: 1, onPage: pageSize });
		props.setLoading(true);
	}

	const handleColumnClick = (params) => {
		if (params.colDef.filterIndex !== undefined) {
			props.setParams({ ...props.params, page: 1, filter: params.colDef.filterIndex });
			props.setLoading(true);
		}
	}

	return (
		<>
			<Typography variant='subtitle1' className={classes.text}>
				Данные отсортированы по убыванию значения в столбце "{columns[props.params.filter + 1].headerName}"
			</Typography>
			<Box className={classes.container}>
				<DataGrid
					rows={getRows()}
					columns={columns}
					page={props.params.page - 1}
					onPageChange={handlePageChange}
					pageSize={props.params.onPage}
					onPageSizeChange={handleOnPageChange}
					rowsPerPageOptions={[10, 20, 50]}
					rowCount={props.total}
					onColumnHeaderClick={handleColumnClick}
					loading={props.loading}
					paginationMode='server'
					disableColumnMenu
					disableSelectionOnClick
					style={{ fontSize: 'medium', }}
				/>
			</Box>
		</>
	)
}

export default FilmStatisticTable