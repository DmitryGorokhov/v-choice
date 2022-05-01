import React from 'react';
import { DataGrid } from '@mui/x-data-grid';
import { createStyles, makeStyles, Box, Typography } from '@material-ui/core';

const useStyles = makeStyles((theme) => createStyles({
	container: {
		height: '600px',
		width: '100%'
	},
	text: {
		margin: theme.spacing(1),
	},
}));

function GenreStatisticTable(props) {
	const classes = useStyles();

	const columns = [
		{
			field: 'value',
			headerName: 'Название',
			align: 'left',
			filterable: false,
			headerAlign: 'left',
			sortable: false,
			width: 700,
		},
		{
			field: 'requested',
			headerName: 'Запросов в фильтре',
			filterIndex: 0,
			align: 'center',
			filterable: false,
			headerAlign: 'center',
			sortable: false,
			type: 'number',
			width: 200,
		},
		{
			field: 'countFilms',
			headerName: 'Количество фильмов',
			filterIndex: 1,
			align: 'center',
			filterable: false,
			headerAlign: 'center',
			sortable: false,
			type: 'number',
			width: 200,
		},
	];

	const getRows = () => {
		const arr = [];
		props.data.forEach((item, index) => {
			arr.push({
				id: index,
				value: item.value,
				requested: item.requested,
				countFilms: item.countFilms,
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

export default GenreStatisticTable