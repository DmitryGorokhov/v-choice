import React, { useEffect, useState } from 'react'
import {
	createStyles,
	makeStyles,
	Box,
	List,
	ListItem,
	Typography,
	InputLabel,
	MenuItem,
	FormHelperText,
	FormControl,
	Select
} from '@material-ui/core'
import Pagination from '@material-ui/lab/Pagination'
import { useHistory } from 'react-router-dom';

import FilmCard from '../../card&tiles/FilmCard/FilmCard'
import AddFilmDialog from '../../crud/AddFilmDialog/AddFilmDialog'
import FilmsFilter from '../../atoms/FilmsFilter/FilmsFilter'

const useStyles = makeStyles((theme) => createStyles({
	filmListItem: {
		display: 'block'
	},
	loading: {
		margin: theme.spacing(1),
		fontSize: '20px'
	},
	tools: {
		display: 'flex',
		justifyContent: 'space-between',
		alignItems: 'center',
		margin: theme.spacing(0, 2),
	}
}));

function FilmList(props) {
	const classes = useStyles();
	const history = useHistory();

	const [state, setState] = useState({
		films: [],
		loading: true,
		totalFilms: 0,
		onPage: props.onPage,
		currentPage: props.pageNumber,
	});

	useEffect(() => {
		fetch(`https://localhost:5001/api/film?pagenumber=${state.currentPage}&onpagecount=${state.onPage}`)
			.then(response => response.json())
			.then(result => {
				setState({ ...state, films: result.items, totalFilms: result.totalCount, loading: false });
			});
	}, [state.currentPage, state.onPage])

	const updateFilmList = (filmList) => {
		setState({ films: filmList });
	}

	const showAll = () => {

	}

	const handleChangePage = (event, newPage) => {
		history.replace({ pathname: `/catalog/${newPage}/${state.onPage}` });
		setState({ ...state, currentPage: newPage, loading: true });
	}

	const handleChangeOnPageCount = (event) => {
		const newCountOnPage = event.target.value;
		history.replace({ pathname: `/catalog/${1}/${newCountOnPage}` });
		setState({ ...state, currentPage: 1, onPage: newCountOnPage, loading: true });
	}

	const calculatePagesCount = () => {
		let value = Math.floor(state.totalFilms / state.onPage);
		return value * state.onPage === state.totalFilms ? value : value + 1;
	}

	return (
		<>
			{
				state.loading
					? <Typography className={classes.loading}>
						Загрузка...
					</Typography >
					: <>
						<Box>
							<Box className={classes.tools}>
								<Typography variant="subtitle1">
									Инструменты
								</Typography>
								<FilmsFilter
									onFilter={updateFilmList}
									genres={props.genres}
									loadAll={showAll}
								/>
								<AddFilmDialog genres={props.genres} />
							</Box>
							<Box>
								<List>
									{
										state.films.length !== 0
											? state.films.map(film => {
												return (
													<ListItem
														className={classes.filmListItem}
														key={film.id}
													>
														<FilmCard film={film} />
													</ListItem>
												)
											})
											: <Typography variant="h5">
												Нет фильмов с выбранным жанром
											</Typography>
									}
								</List>
							</Box>
						</Box>
						<Box>
							<Pagination
								page={Number(state.currentPage)}
								count={calculatePagesCount()}
								variant="outlined"
								color="primary"
								onChange={handleChangePage}
							/>
							<FormControl sx={{ m: 1, minWidth: 120 }}>
								<InputLabel id="simple-select-helper-label">Количество</InputLabel>
								<Select
									labelId="simple-select-helper-label"
									id="simple-select-helper"
									value={state.onPage}
									label="Количество"
									onChange={handleChangeOnPageCount}
								>
									<MenuItem value={3}>3</MenuItem>
									<MenuItem value={5}>5</MenuItem>
									<MenuItem value={10}>10</MenuItem>
									<MenuItem value={20}>20</MenuItem>
									<MenuItem value={50}>50</MenuItem>
								</Select>
								<FormHelperText>фильмов на странице</FormHelperText>
							</FormControl>
						</Box>
					</>
			}
		</>
	)
}

export default FilmList
