import React, { useContext, useEffect, useState } from 'react'
import { createStyles, makeStyles, Box, Button, Collapse, List, ListItem, Typography, } from '@material-ui/core'
import Pagination from '@material-ui/lab/Pagination'
import { useHistory } from 'react-router-dom'
import ExpandLessIcon from '@material-ui/icons/ExpandLess'
import ExpandMoreIcon from '@material-ui/icons/ExpandMore'

import FilmCard from '../../card&tiles/FilmCard/FilmCard'
import AddFilmDialog from '../../crud/AddFilmDialog/AddFilmDialog'
import FilmsFilter from '../../atoms/FilmsFilter/FilmsFilter'
import GenreManager from '../../crud/GenresManager/GenresManager'
import { SortingType } from '../../enums/SortingType'
import { FilteringType } from '../../enums/FilteringType'
import { QueryProps } from '../../enums/QueryProps'
import OnPageCountSwitcher from '../../atoms/OnPageCountSwitcher/OnPageCountSwitcher'
import UserContext from '../../../context';

const useStyles = makeStyles((theme) => createStyles({
	filmListItem: {
		display: 'block'
	},
	loading: {
		margin: theme.spacing(1),
	},
	tools: {
		display: 'flex',
		justifyContent: 'center',
		alignItems: 'center',
	},
	pagination: {
		margin: theme.spacing(1, 0),
		display: 'flex',
		flexDirection: 'row',
		justifyContent: 'center',
		alignItems: 'center',
	},
	paginationLeftItem: {
		marginRight: theme.spacing(4),
	},
	toolsContainer: {
		margin: theme.spacing(0, 0, 3),
	},
	header: {
		display: 'flex',
		justifyContent: 'space-around',
		alignItems: 'center',
	}
}));

function FilmList(props) {
	const classes = useStyles();
	const history = useHistory();
	const { user, _ } = useContext(UserContext);

	const [state, setState] = useState({
		films: [],
		loading: true,
		totalFilms: 0,
		currentPage: props.page,
		onPage: props.count,
		countPages: 0,
		byGenreId: props.genre,
		sortingType: props.sortingType,
		withCommentsOnly: props.withCommentsOnly,
		withRateOnly: props.withRateOnly,
	});

	const [reload, setReload] = useState(false);
	const [open, setOpen] = useState(true);

	const calculatePagesCount = (total, onPage) => {
		let value = Math.floor(total / onPage);
		return value * onPage === total ? value : value + 1;
	}

	useEffect(() => {
		fetch("https://localhost:5001/api/Film?" +
			`PageNumber=${state.currentPage}` +
			`&OnPageCount=${state.onPage}` +
			`&GenreId=${state.byGenreId}` +
			`&SortBy=${state.sortingType}` +
			`&HasCommentsOnly=${state.withCommentsOnly}` +
			`&HasRateOnly=${state.withRateOnly}`
		)
			.then(response => response.json())
			.then(result => {
				setState({
					...state,
					films: result.items,
					totalFilms: result.totalCount,
					countPages: calculatePagesCount(result.totalCount, state.onPage),
					loading: false
				});
				setReload(false);
			});
	}, [
		state.currentPage,
		state.onPage,
		state.byGenreId,
		state.sortingType,
		state.commonOrder,
		state.withCommentsOnly,
		state.withRateOnly,
		reload
	])

	const createCatalogURL = (
		p,
		c,
		g = 0,
		sort = SortingType['not-set'],
		withCommentsOnly = false,
		withRateOnly = false
	) => {
		let url = `/catalog/${QueryProps.Page}=${p}&${QueryProps.Count}=${c}`;

		if (g && g !== 0) {
			url += `&${QueryProps.GenreId}=${g}`;
		}

		if (sort && sort !== SortingType['not-set']) {
			url += `&${QueryProps.SortBy}=${sort}`;
		}

		let filter = FilteringType.NotSet;
		if (withCommentsOnly || withRateOnly) {
			if (withCommentsOnly) {
				filter = withRateOnly ? FilteringType.RatedCommented : FilteringType.Commented;
			}
			else {
				filter = FilteringType.Rated;
			}
		}

		if (filter !== FilteringType.NotSet) {
			url += `&${QueryProps.Filter}=${filter}`;
		}

		return url;
	}

	const handleFiltersChanged = (g, s, cf, rf) => {
		history.replace({ pathname: createCatalogURL(1, state.onPage, g, s, cf, rf) })
		setState({
			...state,
			currentPage: 1,
			byGenreId: g,
			sortingType: s,
			withCommentsOnly: cf,
			withRateOnly: rf,
			loading: true
		});
	}

	const handleChangePage = (_, newPage) => {
		if (newPage === state.currentPage) {
			setState({ ...state, loading: true });
			setReload(true);
		}
		else {
			history.replace({
				pathname: createCatalogURL(
					newPage,
					state.onPage,
					state.byGenreId,
					state.sortingType,
					state.withCommentsOnly,
					state.withRateOnly
				)
			})
			setState({ ...state, currentPage: newPage, loading: true })
		}
	}

	const handleChangeOnPageCount = (event) => {
		const newCount = event.target.value;
		history.replace({
			pathname: createCatalogURL(
				1,
				newCount,
				state.byGenreId,
				state.sortingType,
				state.withCommentsOnly,
				state.withRateOnly
			)
		});
		setState({ ...state, currentPage: 1, onPage: newCount, loading: true });
	}

	const handleCreateFilm = () => {
		const total = state.totalFilms + 1;
		setState({ ...state, totalFilms: total, countPages: calculatePagesCount(total, state.onPage) });
	}

	const handleUpdateFilm = (film) => {
		let arr = [...state.films];
		let found = arr.find(f => f.id === film.id);
		if (found) {
			found = { ...film };
		}
		setState({ ...state, films: [...arr] });
	}

	const handleDeleteFilm = (_) => {
		const total = state.totalFilms - 1;
		const pages = calculatePagesCount(total, state.onPage);
		const currentPage = pages < state.countPages && state.currentPage > pages ? pages : state.currentPage;
		handleChangePage({}, currentPage);
	}

	return (
		<>
			<Box className={classes.header}>
				<Typography variant="h4">Каталог фильмов</Typography>
				<Button onClick={() => setOpen(!open)} size="small">
					{open ? <ExpandLessIcon fontSize="small" /> : <ExpandMoreIcon fontSize="small" />}
					{open ? "Свернуть элементы управления" : "Развернуть элементы управления"}
				</Button>
			</Box>
			{
				state.loading
					? <Typography className={classes.loading} variant='subtitle1'>Загрузка...</Typography >
					: <>
						<Collapse in={open} timeout="auto">
							<Box className={classes.toolsContainer}>
								<Box className={classes.tools}>
									<FilmsFilter
										onSubmit={handleFiltersChanged}
										genres={props.genres}
										selectedGenre={state.byGenreId}
										selectedSortType={state.sortingType}
										selectedCF={state.withCommentsOnly}
										selectedRF={state.withRateOnly}
									/>
								</Box>
								{
									user.isAdmin
										? <Box className={classes.tools}>
											<AddFilmDialog genres={props.genres} onCreate={handleCreateFilm} />
											<GenreManager
												genres={props.genres}
												onCreate={props.onGenreCreate}
												onUpdate={props.onGenreUpdate}
												onDelete={props.onGenreDelete}
											/>
										</Box>
										: null
								}
							</Box>
							{
								state.films.length !== 0
									? <Box className={classes.pagination}>
										<Box className={classes.paginationLeftItem}>
											<Pagination
												page={Number(state.currentPage)}
												count={state.countPages}
												variant="outlined"
												color="primary"
												onChange={handleChangePage}
											/>
										</Box>
										<OnPageCountSwitcher count={state.onPage} onChange={handleChangeOnPageCount} />
									</Box>
									: null
							}
						</Collapse>

						<List>
							{
								state.films.length !== 0
									? state.films.map(film => {
										return (
											<ListItem
												className={classes.filmListItem}
												key={film.id}
											>
												<FilmCard
													film={film}
													onUpdate={handleUpdateFilm}
													onDelete={handleDeleteFilm}
													genres={props.genres} />
											</ListItem>
										)
									})
									: <Typography variant="h5">
										Не найдено фильмов по запросу
									</Typography>
							}
						</List>
						{
							state.films.length !== 0
								? <Box className={classes.pagination}>
									<Box className={classes.paginationLeftItem}>
										<Pagination
											page={Number(state.currentPage)}
											count={state.countPages}
											variant="outlined"
											color="primary"
											onChange={handleChangePage}
										/>
									</Box>
									<OnPageCountSwitcher count={state.onPage} onChange={handleChangeOnPageCount} />
								</Box>
								: null
						}
					</>
			}
		</>
	)
}

export default FilmList
