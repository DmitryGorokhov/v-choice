import React from 'react';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';

export default function FormDialog(props) {
	const [open, setOpen] = React.useState(false);
	let onAddFilm = props.foo;

	let film = {
		Title: '',
		Description: '',
		Year: ''
	}

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
					<TextField
						autoFocus
						margin="dense"
						id="title"
						label="Название"
						type="input"
						onChange={handleChangeTitle}
					/>
					<TextField
						margin="dense"
						id="year"
						label="Год"
						type="number"
						onChange={handleChangeYear}
					/>
					<TextField
						margin="dense"
						id="description"
						label="Описание"
						type="input"
						onChange={handleChangeDescription}
						fullWidth
					/>
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
