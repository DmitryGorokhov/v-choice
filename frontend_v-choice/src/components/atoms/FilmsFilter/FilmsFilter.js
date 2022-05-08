import React, { useState } from 'react'
import { makeStyles } from '@material-ui/core/styles'
import {
	Button,
	Checkbox,
	FormControl,
	FormControlLabel,
	FormHelperText,
	InputLabel,
	MenuItem,
	Select
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
	selectEmpty: {
		marginTop: theme.spacing(2),
	},
}));

function FilmsFilter(props) {
	const classes = useStyles();
	const [state, setState] = useState({
		byGenreId: props.selectedGenre,
		sortingType: props.selectedSortType,
		withCommentsOnly: props.selectedCF,
		withRateOnly: props.selectedRF
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

	const handleSubmit = () => {
		setParamsChanged(false);
		props.onSubmit(
			state.byGenreId,
			state.sortingType,
			state.withCommentsOnly,
			state.withRateOnly
		);
	};

	return (
		<div className={classes.container}>
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
			<Button onClick={handleSubmit} disabled={!paramsChanged}>
				<SearchIcon />
			</Button>
		</div>
	)
}

export default FilmsFilter
