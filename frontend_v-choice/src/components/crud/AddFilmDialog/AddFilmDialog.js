import React from 'react'
import {
	createStyles,
	makeStyles,
	Box,
	Button,
	Checkbox,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	DialogContentText,
	FormControlLabel,
	TextField,
	Typography
} from '@material-ui/core'
import AddIcon from '@material-ui/icons/Add'

import MyAlerter from '../../atoms/MyAlerter/MyAlerter'

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
	const [error, setError] = React.useState(null);
	const [msg, setMsg] = React.useState(null);

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
		const postURL = 'https://localhost:5001/api/films';
		fetch(postURL, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json;charset=utf-8'
			},
			body: JSON.stringify(film)
		})
			.then(response => {
				if (response.status === 401) {
					setError("Недостаточно прав для выполнения операции");
				}
				if (response.status === 201) {
					setMsg("Фильм успешно создан");
				}
			});
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
			? film.genres.push(genres[event.target.name])
			: film.genres.splice(film.genres.indexOf(genres[event.target.name]), 1);
	};

	return (
		<div>
			<Button variant="outlined" color="primary" onClick={handleClickOpen}>
				<AddIcon />
				Добавить фильм
			</Button>

			<Dialog open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
				<DialogTitle id="form-dialog-title">Добавить новый фильм</DialogTitle>
				<DialogContent>
					<MyAlerter msg={msg} error={error} />
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
										label={g.value}
									/>
								);
							})
						}
					</Box>
				</DialogContent>
				<DialogActions>
					<Button onClick={handleClose} color="primary">
						Закрыть
					</Button>
					<Button onClick={handleSubmit} color="primary">
						Добавить
					</Button>
				</DialogActions>
			</Dialog>
		</div>
	);
}
