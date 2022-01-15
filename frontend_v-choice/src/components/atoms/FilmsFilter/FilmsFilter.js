import React, { useState } from 'react'
import { makeStyles } from '@material-ui/core/styles'
import {
	Button,
	FormControl,
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
	});

	const handleChangeGenreId = (event) => {
		setState({ ...state, byGenreId: Number(event.target.value) });
	};

	const handleSubmit = () => {
		state.byGenreId === -1
			? props.loadAll()
			: props.onFilter(state.byGenreId);
	};

	return (
		<div className={classes.container}>
			<FormControl className={classes.formControl}>
				<InputLabel id="demo-simple-select-label">Жанр</InputLabel>
				<Select
					labelId="demo-simple-select-label"
					id="demo-simple-select"
					value={state.byGenreId}
					onChange={handleChangeGenreId}
				>
					<MenuItem value="-1">
						<em>Выберите жанр</em>
					</MenuItem>
					{
						props.genres.map(g => {
							return <MenuItem value={g.id} key={g.id}>{g.value}</MenuItem>
						})
					}
					<MenuItem value="-1">Показать все</MenuItem>
				</Select>
				<FormHelperText>Фильтр по жанру</FormHelperText>
			</FormControl>
			<Button onClick={handleSubmit}>
				<SearchIcon />
			</Button>
		</div>
	)
}

export default FilmsFilter
