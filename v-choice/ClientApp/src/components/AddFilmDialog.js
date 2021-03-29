import React from 'react';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';
import { Box, createStyles, makeStyles, Typography } from '@material-ui/core';

const useStyles = makeStyles((theme) => createStyles({
	flex: {
		display: 'flex',
		justifyContent: 'space-between'
	},
	inputTitle: {
		width: theme.spacing(40)
	},
	inputYear: {
		width: theme.spacing(10)
	},
	item: {
		margin: theme.spacing(2, 0)
	},
}));

export default function FormDialog(props) {
	const classes = useStyles();

	const [open, setOpen] = React.useState(false);
	let onAddFilm = props.foo;
	let genres = props.genres;

	let film = {
		Title: '',
		Description: '',
		Year: '',
		Genres: []
	}

	const getAllUnchecked = () => {
		let arr = [];
		genres.forEach(element => {
			arr.push(false)
		});
		return (arr);
	};

	const [checked, setChecked] = React.useState(getAllUnchecked());

	const handleClickOpen = () => {
		setOpen(true);
	};

	const handleClose = () => {
		setOpen(false);
	};

	const handleSubmit = () => {
		const postURL = 'api/films';
		fetch(postURL, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json;charset=utf-8'
			},
			body: JSON.stringify(film)
		})
			.then(response => response.json())
			.then(result => onAddFilm(result));
		setChecked(getAllUnchecked());
		setOpen(false);
	};

	const handleChangeTitle = (event) => {
		film.Title = event.target.value;
	};

	const handleChangeYear = (event) => {
		film.Year = event.target.value;
	};

	const handleChangeDescription = (event) => {
		film.Description = event.target.value;
	};

	const handleChange = (event) => {
		checked[event.target.name] = !checked[event.target.name];
		setChecked(checked);
		checked[event.target.name]
			? film.Genres.push(genres[event.target.name])
			: film.Genres.splice(film.Genres.indexOf(genres[event.target.name]), 1);
	};

	return (
		<div>
			<Button variant="outlined" color="primary" onClick={handleClickOpen}>
				Добавить новый
      		</Button>

			<Dialog open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
				<DialogTitle id="form-dialog-title">Добавить новый фильм</DialogTitle>
				<DialogContent>
					<DialogContentText>
						Заполните информацию о фильме
          			</DialogContentText>
					<Box className={classes.item && classes.flex}>
						<TextField
							autoFocus
							margin="dense"
							id="title"
							label="Название"
							type="input"
							className={classes.inputTitle}
							onChange={handleChangeTitle}
						/>
						<TextField
							margin="dense"
							id="year"
							label="Год"
							type="number"
							className={classes.inputYear}
							onChange={handleChangeYear}
						/>
					</Box>

					<TextField
						margin="dense"
						id="description"
						label="Описание"
						type="input"
						className={classes.item}
						onChange={handleChangeDescription}
						fullWidth
					/>
					<Box className={classes.item}>
						<Typography>Жанры</Typography>
						{
							genres.map((g, index) => {
								return (
									<FormControlLabel
										key={index}
										control={
											<Checkbox
												checked={checked[index]}
												onChange={handleChange}
												name={`${index}`}
												color="primary"
											/>
										}
										label={g.Value}
									/>
								);
							})
						}
					</Box>
				</DialogContent>
				<DialogActions>
					<Button onClick={handleClose} color="primary">
						Отменить
          			</Button>
					<Button onClick={handleSubmit} color="primary">
						Добавить
          			</Button>
				</DialogActions>
			</Dialog>
		</div>
	);
}
