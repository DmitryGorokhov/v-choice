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
		byGenreId: props.selectedGenre !== undefined ? props.selectedGenre : -1,
		sortType: 0,
		commonOrder: true,
		hasCommentsOnly: false,
		withoutUserRate: false
	});

	const handleChangeGenreId = (event) => {
		setState({ ...state, byGenreId: Number(event.target.value) });
	};

	const handleSubmit = () => {
		props.onFilter(
			state.byGenreId,
			state.sortType,
			state.commonOrder,
			state.hasCommentsOnly,
			state.withoutUserRate);
	};

	const handleSortTypeChanged = (event) => {
		const value = Number(event.target.value)
		if (value !== 0) {
			setState({
				...state,
				commonOrder: Math.floor(value / 10),
				sortType: Boolean(value % 10)
			});
		}
	};

	const handleWithoutRateChanged = (event) => {
		setState({ ...state, withoutUserRate: !state.withoutUserRate })
	}

	const handleHasCommentsOnlyChanged = (event) => {
		setState({ ...state, hasCommentsOnly: !state.hasCommentsOnly })
	}

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
					value={-1}
					onChange={handleSortTypeChanged}
				>
					<MenuItem value="0">Выберите вариант</MenuItem>
					<MenuItem value="10">По дате создания: сначала старые</MenuItem>
					<MenuItem value="11">По дате создания: сначала новые</MenuItem>
					<MenuItem value="20">По году выхода: сначала старые</MenuItem>
					<MenuItem value="21">По году выхода: сначала новые</MenuItem>
					<MenuItem value="30">По возрастанию величины рейтинга</MenuItem>
					<MenuItem value="31">По убыванию величины рейтинга</MenuItem>
					<MenuItem value="0">Показать все</MenuItem>
				</Select>
				<FormHelperText>Сортировка каталога</FormHelperText>
			</FormControl>
			<FormControl className={classes.formControl}>
				<FormControlLabel
					control={<Checkbox
						checked={state.hasCommentsOnly}
						onChange={handleHasCommentsOnlyChanged}
						color="primary" />}
					label="Только с комментариями" />
				<FormControlLabel
					control={<Checkbox
						checked={state.withoutUserRate}
						onChange={handleWithoutRateChanged}
						color="primary" />}
					label="Без мой оценки" />
			</FormControl>
			<Button onClick={handleSubmit}>
				<SearchIcon />
			</Button>
		</div>
	)
}

export default FilmsFilter
