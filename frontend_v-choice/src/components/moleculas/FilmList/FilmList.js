import React, { Component } from 'react'
import { withRouter } from 'react-router';
import { Box, List, ListItem, Typography, InputLabel, MenuItem, FormHelperText, FormControl, Select } from '@material-ui/core'
import { withStyles } from "@material-ui/core/styles"
import Pagination from '@material-ui/lab/Pagination'

import FilmCard from '../../card&tiles/FilmCard/FilmCard'
import AddFilmDialog from '../../crud/AddFilmDialog/AddFilmDialog'
import FilmsFilter from '../../atoms/FilmsFilter/FilmsFilter'

const styles = (theme) => ({
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
});

class FilmList extends Component {
	constructor(props) {
		super(props);
		this.state = {
			genres: [],
			films: [],
			loading: true,
			totalFilms: 0
		};
		this.allFilms = [];
		this.onPage = this.props.onPage;
	}

	componentDidMount() {
		this.fetchGenresData();
		this.fetchFilmsData();
	}

	async fetchFilmsData() {
		fetch(`https://localhost:5001/api/film?pagenumber=${this.props.pageNumber}&onpagecount=${this.onPage}`)
			.then(response => response.json())
			.then(result => {
				this.allFilms = result.items;
				this.setState({ films: result.items, totalFilms: result.totalCount });
			});
	}

	async fetchGenresData() {
		fetch('https://localhost:5001/api/genre')
			.then(response => response.json())
			.then(result => this.setState({ genres: result, loading: false }));
	}

	updateFilmList = (filmList) => {
		this.setState({ films: filmList })
	}

	showAll = () => {
		this.setState({ films: this.allFilms });
	}

	handleChangePage = (event, newPage) => {
		this.props.history.push(`/catalog/${newPage}/${this.onPage}`);
		this.props.history.go();
	}

	handleChangeOnPageCount = (event) => {
		this.props.history.push(`/catalog/${1}/${event.target.value}`);
		this.props.history.go();
	}

	calculatePagesCount = () => {
		let value = Math.floor(this.state.totalFilms / this.onPage)
		return value * this.onPage === this.state.totalFilms ? value : value + 1
	}

	render() {
		return (
			<>
				{
					this.state.loading
						? <Typography className={this.props.classes.loading}>
							Загрузка...
						</Typography >
						: <>
							<Box>
								<Box className={this.props.classes.tools}>
									<Typography variant="subtitle1">
										Инструменты
									</Typography>
									<FilmsFilter
										onFilter={this.updateFilmList}
										genres={this.state.genres}
										loadAll={this.showAll}
									/>
									<AddFilmDialog genres={this.state.genres} />
								</Box>
								<Box>
									<List>
										{
											this.state.films.length !== 0
												? this.state.films.map(film => {
													return (
														<ListItem
															className={this.props.classes.filmListItem}
															key={film.Id}
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
									page={Number(this.props.pageNumber)}
									count={this.calculatePagesCount()}
									variant="outlined"
									color="primary"
									onChange={this.handleChangePage}
								/>
								<FormControl sx={{ m: 1, minWidth: 120 }}>
									<InputLabel id="simple-select-helper-label">Количество</InputLabel>
									<Select
										labelId="simple-select-helper-label"
										id="simple-select-helper"
										value={this.onPage}
										label="Количество"
										onChange={this.handleChangeOnPageCount}
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
}

export default withRouter(withStyles(styles)(FilmList))
