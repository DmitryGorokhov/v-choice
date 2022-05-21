import React, { useState } from 'react'
import { makeStyles } from '@material-ui/core/styles'
import {
	Box,
	Button,
	Checkbox,
	FormControl,
	FormControlLabel,
	FormHelperText,
	InputLabel,
	MenuItem,
	Select,
	Slider,
	TextField,
	Typography
} from '@material-ui/core'
import SearchIcon from '@material-ui/icons/Search'
import { SortingType } from '../../enums/SortingType'

const useStyles = makeStyles((theme) => ({
	container: {
		display: 'flex',
		alignItems: 'center',
	},
	formControl: {
		margin: theme.spacing(1),
		minWidth: 150,
	},
	searchContainer: {
		display: 'flex',
		justifyContent: 'space-around',
		width: '100%',
	},
	searchControl: {
		margin: theme.spacing(1),
		minWidth: 300,
	},
	selectEmpty: {
		marginTop: theme.spacing(2),
	},
	stack: {
		display: 'flex',
		flexDirection: 'column',
	},
}));

function FilmsFilter(props) {
	const classes = useStyles();
	const [state, setState] = useState({
		byGenreId: props.selected.genre,
		sortingType: props.selected.sortType,
		withCommentsOnly: props.selected.cF,
		withRateOnly: props.selected.rF,
		byYear: props.selected.yearRange,
		byRate: props.selected.rateRange,
		byActorId: props.selected.a,
		byDirectorId: props.selected.d,
		byStudioId: props.selected.s,
		search: props.selected.search,
	});

	const [paramsChanged, setParamsChanged] = useState(false);

	const handleChangeGenreId = (event) => {
		setParamsChanged(true);
		setState({ ...state, byGenreId: Number(event.target.value) });
	};

	const handleSortingTypeChanged = (event) => {
		setParamsChanged(true);
		setState({ ...state, sortingType: event.target.value });
	};

	const handleCommentsFilterChanged = (_) => {
		setParamsChanged(true);
		setState({ ...state, withCommentsOnly: !state.withCommentsOnly })
	};

	const handleRateFilterChanged = (_) => {
		setParamsChanged(true);
		setState({ ...state, withRateOnly: !state.withRateOnly })
	};

	const handleDirectorChanged = (event) => {
		setParamsChanged(true);
		setState({ ...state, byDirectorId: Number(event.target.value) });
	}

	const handleActorChanged = (event) => {
		setParamsChanged(true);
		setState({ ...state, byActorId: Number(event.target.value) });
	}

	const handleStudioChanged = (event) => {
		setParamsChanged(true);
		setState({ ...state, byStudioId: Number(event.target.value) });
	}

	const handleYearRangeChanged = (_, newValues) => {
		setParamsChanged(true);
		setState({ ...state, byYear: { min: newValues[0], max: newValues[1] } });
	}

	const handleRateRangeChanged = (_, newValues) => {
		setParamsChanged(true);
		setState({ ...state, byRate: { min: newValues[0], max: newValues[1] } });
	}

	const handleSearchChanged = (event) => {
		setParamsChanged(true);
		setState({ ...state, search: event.target.value.substr(0, 40) });
	}

	const handleSubmit = () => {
		setParamsChanged(false);
		props.onSubmit(
			state.byGenreId,
			state.sortingType,
			state.withCommentsOnly,
			state.withRateOnly,
			state.byYear,
			state.byRate,
			state.byActorId,
			state.byDirectorId,
			state.byStudioId,
			state.search
		);
	};

	return (
		<Box className={classes.stack}>
			<Box className={classes.container}>
				<Box className={classes.searchContainer}>
					<TextField
						id="search"
						label="Поиск"
						variant="standard"
						value={state.search}
						onChange={handleSearchChanged}
						className={classes.searchControl}
					/>
					<Button onClick={handleSubmit} disabled={!paramsChanged}>
						<SearchIcon />
					</Button>
				</Box>
			</Box>
			<Box className={classes.container}>
				<Box className={classes.formControl}>
					<Typography gutterBottom variant='body1'>Год создания</Typography>
					<Slider
						value={[state.byYear.min, state.byYear.max]}
						onChange={handleYearRangeChanged}
						valueLabelDisplay="auto"
						min={props.filterData.yearMin}
						max={props.filterData.yearMax}
					/>
				</Box>
				<FormControl className={classes.formControl}>
					<InputLabel id="label-select-genre">Жанр</InputLabel>
					<Select
						labelId="label-select-genre"
						id="select-genre"
						value={state.byGenreId}
						onChange={handleChangeGenreId}
					>
						<MenuItem value="0">
							<em>Выберите жанр</em>
						</MenuItem>
						{
							props.genres.map(g => {
								return <MenuItem value={g.id} key={g.id}>{g.value}</MenuItem>
							})
						}
						<MenuItem value="0">Показать все</MenuItem>
					</Select>
					<FormHelperText>Фильтр по жанру</FormHelperText>
				</FormControl>
				<FormControl className={classes.formControl}>
					<InputLabel id="label-select-sort">Отсортировать</InputLabel>
					<Select
						labelId="label-select-sort"
						id="select-sort"
						value={state.sortingType}
						onChange={handleSortingTypeChanged}
					>
						<MenuItem value={SortingType['not-set']}>Выберите вариант</MenuItem>
						<MenuItem value={SortingType.created}>По дате создания: сначала старые</MenuItem>
						<MenuItem value={SortingType['created-desc']}>По дате создания: сначала новые</MenuItem>
						<MenuItem value={SortingType.year}>По году выхода: сначала старые</MenuItem>
						<MenuItem value={SortingType['year-desc']}>По году выхода: сначала новые</MenuItem>
						<MenuItem value={SortingType.rate}>По возрастанию величины рейтинга</MenuItem>
						<MenuItem value={SortingType['rate-desc']}>По убыванию величины рейтинга</MenuItem>
						<MenuItem value={SortingType['not-set']}>Показать все</MenuItem>
					</Select>
					<FormHelperText>Сортировка каталога</FormHelperText>
				</FormControl>
				<FormControl className={classes.formControl}>
					<FormControlLabel
						control={<Checkbox
							checked={state.withCommentsOnly}
							onChange={handleCommentsFilterChanged}
							color="primary" />}
						label="Только с комментариями" />
					<FormControlLabel
						control={<Checkbox
							checked={state.withRateOnly}
							onChange={handleRateFilterChanged}
							color="primary" />}
						label="Только с оценкой" />
				</FormControl>
			</Box>
			<Box className={classes.container}>
				<Box className={classes.formControl}>
					<Typography gutterBottom variant='body1'>Рейтинг</Typography>
					<Slider
						value={[state.byRate.min, state.byRate.max]}
						onChange={handleRateRangeChanged}
						valueLabelDisplay="auto"
						min={Math.floor(props.filterData.rateMin)}
						max={Math.floor(props.filterData.rateMax) + 1}
						step={0.5}
					/>
				</Box>
				<FormControl className={classes.formControl}>
					<InputLabel id="label-select-director">Режиссёр</InputLabel>
					<Select
						labelId="label-select-director"
						id="select-director"
						value={state.byDirectorId}
						onChange={handleDirectorChanged}
					>
						<MenuItem value={0}>Не выбран</MenuItem>
						{
							props.persons.map(p => <MenuItem value={p.id}>{p.fullName}</MenuItem>)
						}
					</Select>
				</FormControl>
				<FormControl className={classes.formControl}>
					<InputLabel id="label-select-actor">Актёр</InputLabel>
					<Select
						labelId="label-select-actor"
						id="select-actor"
						value={state.byActorId}
						onChange={handleActorChanged}
					>
						<MenuItem value={0}>Не выбран</MenuItem>
						{
							props.persons.map(p => <MenuItem value={p.id}>{p.fullName}</MenuItem>)
						}
					</Select>
				</FormControl>
				<FormControl className={classes.formControl}>
					<InputLabel id="label-select-studio">Студия</InputLabel>
					<Select
						labelId="label-select-studio"
						id="select-studio"
						value={state.byStudioId}
						onChange={handleStudioChanged}
					>
						<MenuItem value={0}>Не выбран</MenuItem>
						{
							props.studios.map(s => <MenuItem value={s.id}>{s.name}</MenuItem>)
						}
					</Select>
				</FormControl>
			</Box>
		</Box>
	)
}

export default FilmsFilter
