import React from 'react';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import { Box, createStyles, makeStyles, Typography } from '@material-ui/core';
import MyAlerter from './MyAlerter';

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

export default function UpdateFilmDialog(props) {
	const classes = useStyles();
	const [open, setOpen] = React.useState(false);
	const [title, setTitle] = React.useState(props.film.Title);
	const [year, setYear] = React.useState(props.film.Year);
	const [description, setDescription] = React.useState(props.film.Description);

	const [error, setError] = React.useState(null);
	const [msg, setMsg] = React.useState(null);

	const handleClickOpen = () => {
		setOpen(true);
	};

	const handleClose = () => {
		setOpen(false);
	};

	const handleSubmit = () => {
		let film = {
			Title: title || props.film.Title,
			Description: description || props.film.Description,
			Year: year || props.film.Year,
		}
		const postURL = `api/films/${props.film.Id}`;
		fetch(postURL, {
			method: 'PUT',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify(film)
		})
			.then(response => {
				if (response.status === 401) {
					setError("Недостаточно прав для выполнения операции");
				}
				if (response.status === 204) {
					setMsg("Фильм успешно изменен")
				}
			});
	};

	const handleChangeTitle = (event) => {
		setTitle(event.target.value);
	};

	const handleChangeYear = (event) => {
		setYear(event.target.value);
	};

	const handleChangeDescription = (event) => {
		setDescription(event.target.value);
	};

	return (
		<div>
			<Button variant="outlined" color="primary" onClick={handleClickOpen}>
				Изменить
      		</Button>

			<Dialog open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
				<DialogTitle id="form-dialog-title">Изменить информацию о фильме</DialogTitle>
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
							value={title}
							className={classes.inputTitle}
							onChange={handleChangeTitle}
						/>
						<TextField
							margin="dense"
							id="year"
							label="Год"
							type="number"
							value={year}
							className={classes.inputYear}
							onChange={handleChangeYear}
						/>
					</Box>

					<TextField
						margin="dense"
						id="description"
						label="Описание"
						type="input"
						value={description}
						className={classes.item}
						onChange={handleChangeDescription}
						fullWidth
					/>
				</DialogContent>
				<DialogActions>
					<Button onClick={handleClose} color="primary">
						Отменить
          			</Button>
					<Button onClick={handleSubmit} color="primary">
						Применить
          			</Button>
				</DialogActions>
			</Dialog>
		</div>
	);
}
