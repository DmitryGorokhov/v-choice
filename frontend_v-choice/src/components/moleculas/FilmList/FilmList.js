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
		currentPage: props.pageNumber,
		onPage: props.onPage,
		byGenreId: props.genre,
		sortType: props.sortType,
		commonOrder: props.order,
		hasCommentsOnly: props.onlyComments,
		withoutUserRate: props.noUserRate
	});

	useEffect(() => {
		fetch("https://localhost:5001/api/Film?" +
			`PageNumber=${state.currentPage}` +
			`&OnPageCount=${state.onPage}` +
			`&GenreId=${state.byGenreId}` +
			`&SortBy=${state.sortType}` +
			`&CommonOrder=${state.commonOrder}` +
			`&HasCommentsOnly=${state.hasCommentsOnly}` +
			`&WithoutMyRateOnly=${state.withoutUserRate}`
		)
			.then(response => response.json())
			.then(result => {
				setState({
					...state,
					films: result.items,
					totalFilms: result.totalCount,
					loading: false
				});
			});
	}, [
		state.currentPage,
		state.onPage,
		state.byGenreId,
		state.sortType,
		state.commonOrder,
		state.hasCommentsOnly,
		state.withoutUserRate
	])

	const showAll = () => {
		history.replace({ pathname: `/catalog/${1}/${state.onPage}` });
		setState({
			...state,
			currentPage: 1,
			byGenreId: 0,
			sortType: 0,
			commonOrder: true,
			hasCommentsOnly: false,
			withoutUserRate: false,
			loading: true
		});
	}

	const handleFiltersChanged = (genre, type, commonOrder, commentsOnly, withoutUserRate) => {
		const newType = type > 0 && type < 4 ? type : 0
		history.replace({
			pathname:
				'/catalog' +
				`/${1}` +
				`/${state.onPage}` +
				`/${genre}` +
				`/${newType}` +
				`/${Number(commonOrder)}` +
				`/${Number(commentsOnly)}` +
				`/${Number(withoutUserRate)}`
		})
		setState({
			...state,
			currentPage: 1,
			byGenreId: genre,
			sortType: newType,
			commonOrder: commonOrder,
			hasCommentsOnly: commentsOnly,
			withoutUserRate: withoutUserRate,
			loading: true
		});
	}

	const handleChangePage = (event, newPage) => {
		history.replace({
			pathname:
				'/catalog' +
				`/${newPage}` +
				`/${state.onPage}` +
				`/${state.byGenreId}` +
				`/${state.sortType}` +
				`/${Number(state.commonOrder)}` +
				`/${Number(state.hasCommentsOnly)}` +
				`/${Number(state.withoutUserRate)}`
		})
		setState({ ...state, currentPage: newPage, loading: true })
	}

	const handleChangeOnPageCount = (event) => {
		const newCount = event.target.value;
		history.replace({
			pathname:
				'/catalog' +
				`/${1}` +
				`/${newCount}` +
				`/${state.byGenreId}` +
				`/${state.sortType}` +
				`/${Number(state.commonOrder)}` +
				`/${Number(state.hasCommentsOnly)}` +
				`/${Number(state.withoutUserRate)}`
		})
		setState({ ...state, currentPage: 1, onPage: newCount, loading: true });
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
									onFilter={handleFiltersChanged}
									genres={props.genres}
									loadAll={showAll}
									selectedGenre={state.byGenreId}
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
												Не найдено фильмов по запросу
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
								<FormHelperText>на странице</FormHelperText>
							</FormControl>
						</Box>
					</>
			}
		</>
	)
}

export default FilmList
