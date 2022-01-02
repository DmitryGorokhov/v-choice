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
	const [genre, setGenre] = useState('');

	const handleChange = (event) => {
		console.log(event.target.value);
		setGenre(event.target.value);
	};

	const handleSubmit = () => {
		if (genre !== "") {
			genre === '-1'
				? props.loadAll()
				: fetch(`https://localhost:5001/api/genres/${genre}`)
					.then(response => response.json())
					.then(result => props.onFilter(result));
		}
	};

	return (
		<div className={classes.container}>
			<FormControl className={classes.formControl}>
				<InputLabel id="demo-simple-select-label">Жанр</InputLabel>
				<Select
					labelId="demo-simple-select-label"
					id="demo-simple-select"
					value={genre}
					onChange={handleChange}
				>
					<MenuItem value="">
						<em>Выберите жанр</em>
					</MenuItem>
					{
						props.genres.map(g => {
							return <MenuItem value={g.Id} key={g.Id}>{g.value}</MenuItem>
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
