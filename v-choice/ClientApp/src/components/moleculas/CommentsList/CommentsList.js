import React, { Component } from 'react'
import { withStyles } from "@material-ui/core/styles"
import { List, ListItem, Typography } from '@material-ui/core'

import CommentTile from '../../card&tiles/CommentTile/CommentTile'
import CommentArea from '../../atoms/CommentArea/CommentArea'

const styles = (theme) => ({
	listItem: {
		display: 'block'
	},
});

export class CommentsList extends Component {
	constructor(props) {
		super(props);
		this.state = { comments: [], loading: true };
	}

	componentDidMount() {
		this.fetchComments(this.props.filmId);
	}

	async fetchComments(filmId) {
		fetch(`api/comments/${filmId}`)
			.then(response => response.json())
			.then(result => this.setState({ comments: result, loading: false }));
	}

	render() {
		return (
			<div>
				{
					this.state.loading
						? <Typography>Загрузка...</Typography>
						:
						<List>
							{
								this.state.comments.length !== 0
									? this.state.comments.map(comment => {
										return (
											<ListItem className={this.props.classes.listItem} key={comment.Id}>
												<CommentTile comment={comment} userId={null} />
											</ListItem>
										)
									})
									: <Typography variant='h5'>Пока нет комментариев</Typography>
							}
						</List>
				}

				{
					this.props.userEmail === null
						? <Typography variant='h6'>
							Авторизируйтесь, чтобы оставить свой комментарий
							</Typography>
						: <CommentArea />
				}

			</div>
		)
	}
}

export default withStyles(styles)(CommentsList)
